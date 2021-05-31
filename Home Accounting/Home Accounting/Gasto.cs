using System;

public class Gasto : Transaccion
{
    public Gasto()
    {

    }

    public Gasto(int numRegistro, DateTime fecha, string concepto,
        float importe, string categoria, string cuenta, bool estado) :
        base(numRegistro, fecha, concepto, importe, categoria, cuenta)
    {
        this.Estado = true;
    }

    public Gasto(string dato) : base(dato)
    {
    }

    public string PrepararParaGuardar()
    {
        string cadena = "";
        cadena += "G;" + ToDato();

        return cadena;
    }
}
