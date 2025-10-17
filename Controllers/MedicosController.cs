using medicos_c.Data;
using medicos_c.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medicos_c.Controllers;

public class MedicosController : Controller 
{
    // configuracion de la db --> 
    private readonly AppDbContext _context;
    public MedicosController(AppDbContext context)
    {
        _context = context; 
    }
    
    // metodo de mostrar 
    public async Task<IActionResult> Index()
    {
        var medico = await _context.medicos.ToListAsync();
        return View(medico);
    }
    
    //  agergar un nuedo doctor 
    [HttpPost]
    public async Task<IActionResult> Add(Medicos medico)
    {
        try
        {
            bool stadoDocumento = await ValidarDocumento(medico.Documento);
            if (!stadoDocumento)
            {
                TempData["ErrorMessage"] = " Ya existe un Documento con este numero. Intente con otro.";
                return RedirectToAction(nameof(Index));
            }
            await _context.AddAsync(medico);
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
    
    // valida que un docuemto no se repit a
    public async Task<bool> ValidarDocumento(string documento)
    {
        var cosulta = await _context.medicos.FirstOrDefaultAsync(d => d.Documento == documento);
        return cosulta == null;  // ture si no existe (puede crear )
    }
    
    
    // Eliminar 
    public async Task<IActionResult> Delete(int id)
    {
      
        try
        {
            var medico = await _context.medicos.FirstOrDefaultAsync(p => p.Id == id);
            
            if (medico != null)
            {
                _context.Remove(medico);
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
        var consulta = await _context.medicos.FindAsync(id);
        if (consulta == null)
        {
            return NotFound();
        }

        return View("Edit", consulta);
    }    
    
    
    // guardar informacion o actualizar 
   
    [HttpPost]
    public async Task<IActionResult> Update(Medicos medico)
    {
        try
        {
            _context.Update(medico); 
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