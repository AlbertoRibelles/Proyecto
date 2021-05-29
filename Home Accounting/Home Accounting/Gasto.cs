using System;

public class Gasto : IComparable<Gasto>
{
    public DateTime Fecha { get; set; }
    public string Concepto { get; set; }

    public int Id { get; set; }
    public float Importe { get; set; }
    public string Categoria { get; set; }
    public string Cuenta { get; set; }
    public bool Estado { get; set; }

    public Gasto()
    {

    }

    public Gasto(int numRegistro, DateTime fecha, string concepto, 
        float importe, string categoria, string cuenta)
    {
        this.Id = numRegistro; 
        this.Fecha = fecha;
        this.Concepto = concepto;
        this.Importe = importe;
        this.Categoria = categoria;
        this.Cuenta = cuenta;
        this.Estado = true;
    }

    public Gasto(string dato)
    {
        string[] partes = dato.Split(";");
        this.Id = Convert.ToInt32(partes[0]);
        this.Fecha = DateTime.Parse(partes[1]);
        this.Concepto = partes[2];
        this.Importe = Convert.ToSingle(partes[3]);
        this.Categoria = partes[4];
        this.Cuenta = partes[5];
        if (partes[6].Equals("A"))
            this.Estado = true;
        else
            this.Estado = false;
    }

    public override string ToString()
    {
        return ToStringFecha() + " " + Importe + " " + Concepto;
    }

    public string ToDato()
    {
        string cadena = "";
        cadena += Id + ";" + ToStringFecha() + ";"
            + Concepto + ";" + Importe + ";" + Categoria + ";" + Cuenta + ";";
        string dato = Estado == true ? "A" : "D";
        cadena += dato;

        return cadena;
    }

    public virtual string PrepararParaGuardar()
    {
        string cadena= "";
        cadena += "G;" + Id + ";" + ToStringFecha() + ";"
            + Concepto + ";" + Importe + ";" + Categoria + ";" + Cuenta + ";";
        string dato = Estado == true ? "A" : "D";
        cadena += dato;

        return cadena;
    }

    public int CompareTo(Gasto otro)
    {
        return Id.CompareTo(otro.Id);
    }

    public int CompareTo(String categoria)
    {
        return Categoria.CompareTo(categoria);
    }

    public override int GetHashCode()
    {
        return Id;
    }

    public string ToStringFecha()
    {
        return Fecha.Year.ToString() + "/" + Fecha.Month.ToString("00") +
            "/" + Fecha.Day.ToString("00");        
    }

    public bool Contiene(string conceptoABuscar)
    {
        bool encontrado = false;

        if (Concepto.ToUpper().Contains(conceptoABuscar.ToUpper()))
        {
            encontrado = true;
        }
        return encontrado;
    }
}
