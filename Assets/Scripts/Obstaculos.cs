using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstaculos
{
    public bool TienePeatones { get; set; }
    public bool TieneEscuela { get; set; }

    public bool PuedeTransitar() { return TienePeatones; }
    public bool PuedeEstacionar() { return TieneEscuela; }
}
