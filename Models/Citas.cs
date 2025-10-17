using System.ComponentModel.DataAnnotations.Schema;

namespace medicos_c.Models;

public class Citas
{
    public int Id { get; set; }
    
    // realcion de la fk medicos 
    [ForeignKey("Medicos")]
    public int MedicoId { get; set; }
    public Medicos Medicos { get; set; }
    
    
    // hago relacion de la fk pacientes 
    
    [ForeignKey("Pacientes")]
    public int PacienteId { get; set; }
    public virtual Pacientes Pacientes { get; set; }
    
    public DateTime FechaCita { get; set; }
    public string Estado { get; set; }  ="waiting";
}