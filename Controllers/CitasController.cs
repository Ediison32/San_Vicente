using medicos_c.Data;
using medicos_c.Models;
using medicos_c.Utils.EmailSend;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medicos_c.Controllers;

public class CitasController : Controller
{
    // configurguacion db 
    private readonly AppDbContext _context;
    private readonly EmailService _emailService; // para los mensaje s
    
    
    public CitasController(AppDbContext context, EmailService emailService)
    {
        _context = context; 
        _emailService = emailService;
    }
    
    // metodos con all fk de las tablas
    public async Task<IActionResult> Index()
    {
        ViewBag.Medicos = await _context.medicos.ToListAsync();
        ViewBag.Pacientes = await _context.pacientes.ToListAsync();
        var cita = await _context.citas
            .Include(d => d.Medicos)
            .Include(d => d.Pacientes)
            .ToListAsync();
        
        return View(cita);
    }
    
    // añadirt una cit a
    public async Task<IActionResult> Add(Citas cita)
    {
        try
        {
            bool StadoDias = await Validacion(cita.MedicoId, cita.PacienteId, cita.FechaCita);
            if (!StadoDias)
            {
                TempData["ErrorMessage"] = "El medico o paciente ya tiene asignado cita con esta misma fecha.";
                return RedirectToAction(nameof(Index));
            }

            // Guardar la cita
            await _context.AddAsync(cita);
            await _context.SaveChangesAsync();

            
            // 🔁 Recargar cita con las relaciones necesarias
            var citaCompleta = await _context.citas
                .Include(c => c.Pacientes)
                .Include(c => c.Medicos)
                .FirstOrDefaultAsync(c => c.Id == cita.Id);

            
            // Enviar correo solo si los datos están completos
            if (citaCompleta != null && citaCompleta.Pacientes != null && citaCompleta.Medicos != null)
            {
                Console.WriteLine("--------------------------------------- Se fue a enviar el correo ");
                var (status, mensaje) = await _emailService.EnviarConfirmacionCitaAsync(citaCompleta);
                Console.WriteLine(mensaje);
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            TempData["ErrorMessage"] = "Ocurrió un error al crear la cita.";
            return RedirectToAction(nameof(Index));
        }
    }

    
    // mostarar la informacion  para editar
    [HttpGet]
    public async Task<IActionResult> Detalles (int id)
    {
        var reserva = await _context.citas.FindAsync(id);
        if (reserva == null)
            return NotFound();
        
        ViewBag.Medicos = await _context.medicos.ToListAsync();
        ViewBag.Pacientes = await _context.pacientes.ToListAsync();

        return View("Edit", reserva);
    }
    
    //  actualizar informacion 
    
    [HttpPost]

    public async Task<IActionResult> Update(Citas cita)
    {
        try
        {
            _context.Update(cita);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar: {ex.Message}");
              
            ViewBag.Medicos = await _context.medicos.ToListAsync();
            ViewBag.Pacientes = await _context.pacientes.ToListAsync();

            return View("Edit", cita);
        }
    }

    // eliminar 
    public async Task<IActionResult> Delete(int id)
    {
        
        try
        {
            var cita = await _context.citas.FirstOrDefaultAsync(r => r.Id == id);
            if (cita!= null)
            {
                _context.citas.Remove(cita);
                await _context.SaveChangesAsync();
                
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    // metodo cambio de eestado de la cita 

    [HttpPost]
    public async Task<IActionResult> CambioEstado(int id, string estado)
    {
        try
        {
            var cita = _context.citas.FirstOrDefault(c => c.Id == id);
            if (cita == null) return NotFound();
            Console.WriteLine("********************************camio  el estaodo a " + estado);
            cita.Estado = estado;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
    
    // validar las fechas 
    public async Task<bool> Validacion(int medicoId, int pacienteId, DateTime fecha)
    {
        try
        {
            bool validacion = await _context.citas.AnyAsync(v => (v.MedicoId == medicoId || v.PacienteId == pacienteId)
            && v.FechaCita == fecha && v.Estado != "Cancelada");
            
            return !validacion; // si hay una cita retorna   false 
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    // listar por pacientes 

    public async Task<IActionResult> ListarPaciente(int pacienteId)
    {   
        ViewBag.tipo = "Médico";  // Listamos médicos (el "otro lado") para el paciente
        var citas = await _context.citas
            .Include(c => c.Medicos)
            .Include(c => c.Pacientes)
            .Where(c => c.PacienteId == pacienteId)
            .ToListAsync();
        return View("Listar", citas);
    }

    public async Task<IActionResult> ListarMedicos(int medicoId)
    {
        ViewBag.tipo = "Paciente";  // Listamos pacientes para el médico
        var citas = await _context.citas
            .Include(c => c.Medicos)
            .Include(c => c.Pacientes)
            .Where(c => c.MedicoId == medicoId)
            .ToListAsync();
        return View("Listar", citas);
    }



}



// verificar si y evitarr agregar en fechas iguales
// evitando duplicidades y conflictos de
// horarios entre pacientes y médicos 