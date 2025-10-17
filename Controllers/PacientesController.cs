using medicos_c.Data;
using medicos_c.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medicos_c.Controllers;

public class PacientesController : Controller
{
    // configuracion de la db --> 
    private readonly AppDbContext _context;
    public PacientesController(AppDbContext context)  // constructor para la inicializacion 
    {
        _context = context;     
    }
    
    // metodos 

    public async Task<IActionResult> Index()
    {
        var pacientes = await _context.pacientes.ToListAsync();
        return View(pacientes);
    }
    
    // agregar paciente 
    [HttpPost]
    public async Task<IActionResult> Add(Pacientes paciente)
    {
        try
        {
            bool stadoDocumento = await ValidarDocumento(paciente.Documento);
            if (!stadoDocumento)
            {
                TempData["ErrorMessage"] = " Ya existe un Documento con este numero. Intente con otro.";
                return RedirectToAction(nameof(Index));
            }
            await _context.pacientes.AddAsync(paciente);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = " Creado correctamente.";  // para mandar mensajes 
            return RedirectToAction(nameof(Index));

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    // validar que el documento no exista

    
    // valida que un docuemto no se repit a
    public async Task<bool> ValidarDocumento(string documento)
    {
        var cosulta = await _context.pacientes.FirstOrDefaultAsync(d => d.Documento == documento);
        return cosulta == null;  // ture si no existe (puede crear )
    }
    
    
    // Eliminar 
    public async Task<IActionResult> Delete(int id)
    {
      
        try
        {
            var paciente = await _context.pacientes.FirstOrDefaultAsync(p => p.Id == id);
            
            if (paciente != null)
            {
                _context.pacientes.Remove(paciente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));  
            //return View("index");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
    
   
    //  mostrar en la vista para editar 
    [HttpGet]
    public async Task<IActionResult> Detalles(int id)
    {
        var consulta = await _context.pacientes.FindAsync(id);
        if (consulta == null)
        {
            return NotFound();
        }

        return View("Edit", consulta);
    }    

    // guardar informacion o actualizar 
   
    [HttpPost]
    public async Task<IActionResult> Update(Pacientes paciente)
    {
        try
        {
            _context.Update(paciente); 
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar: {ex.Message}");
           
            return View("Edit");
        }
    }
}