using System;
using System.Collections.Generic;
using System.IO;

public class ListaDeGastosYPrevisiones
{
    List<Gasto> gastos;
    List<Prevision> previsiones;

    public ListaDeGastosYPrevisiones()
    {
        gastos = new List<Gasto>();
        previsiones = new List<Prevision>();
    }

    public ListaDeGastosYPrevisiones(string nombreUsuario)
    {
        gastos = new List<Gasto>();
        previsiones = new List<Prevision>();
        Cargar(nombreUsuario);
    }

    public List<Gasto> GetGastosContiene(List<Gasto> g, string conceptoABuscar)
    {
        List<Gasto> auxLista = new List<Gasto>();

        foreach (Gasto gasto in g)
        {
            if (gasto.Contiene(conceptoABuscar))
                auxLista.Add(gasto);
        }
        return auxLista;
    }

    public List<Prevision> GetPrevisionesContiene(
        List<Prevision> p, string conceptoABuscar)
    {
        List<Prevision> auxLista = new List<Prevision>();

        foreach (Prevision prev in p)
        {
            if (prev.Contiene(conceptoABuscar))
                auxLista.Add(prev);
        }
        return auxLista;
    }

    public List<Transaccion> GetTransacciones(List<Gasto> gastos)
    {
        List<Transaccion> auxTrans = new List<Transaccion>();
        
        foreach(Gasto g in gastos)
        {
            auxTrans.Add(g);
        }

        return auxTrans;
    }
    public List<Transaccion> GetTransacciones(List<Prevision> previsones)
    {
        List<Transaccion> auxTrans = new List<Transaccion>();

        foreach (Prevision p in previsones)
        {
            auxTrans.Add(p);
        }

        return auxTrans;
    }

    public List<Gasto> GetGastos()
    {
        return gastos;
    }

    public List<Prevision> GetPrevisiones()
    {
        return previsiones;
    }

    public List<Gasto> GetGastos(
        bool modo, bool ordenacionPorFecha, DateTime fecha)
    {
        List<Gasto> auxLista = new List<Gasto>();

        if (!ordenacionPorFecha)
            OrdenarGastosPorCategoria();
        else
            OrdenarGastosPorFecha();

        foreach (Gasto g in gastos)
        {
            if(g.Fecha.Month.Equals(fecha.Month) && 
                g.Fecha.Year.Equals(fecha.Year))
            {
                if (modo)
                {
                    auxLista.Add(g);
                }

                if(!modo && g.Estado)
                {
                    auxLista.Add(g);
                }
            }
        }

        return auxLista;
    }

    public List<Prevision> GetPrevisiones(
        bool modo, bool ordenacionPorFecha, DateTime fecha)
    {
        List<Prevision> auxLista = new List<Prevision>();

        if (!ordenacionPorFecha)
            OrdenarPrevisionesPorCategoria();
        else
            OrdenarPrevisionesPorFecha();

        foreach (Prevision p in previsiones)
        {
            if (p.Fecha.Month.Equals(fecha.Month) && 
                p.Fecha.Year.Equals(fecha.Year))
            {
                if (modo)
                {
                    auxLista.Add(p);
                }

                if (!modo && p.Estado)
                {
                    auxLista.Add(p);
                }
            }
        }

        return auxLista;
    }

    public int GetProximoIDGasto()
    {
        return gastos.Count + 1;
    }

    public int GetProximoIDPrevision()
    {
        return previsiones.Count + 1;
    }

    public void Anyadir(Gasto g)
    {
        gastos.Add(g);
        OrdenarGastosPorFecha();
    }

    public void Anyadir(Prevision p)
    {
        previsiones.Add(p);
        OrdenarPrevisionesPorFecha();
    }

    public void Modificar(Gasto g)
    {
        gastos.Sort();
        gastos[g.GetHashCode()-1] = g;
        OrdenarGastosPorFecha();
    }
    public void Modificar(Prevision p)
    {
        previsiones.Sort();
        previsiones[p.GetHashCode() - 1] = p;
        OrdenarPrevisionesPorFecha();
    }

    public void Borrar(Gasto g)
    {
        g.Estado = false;
        Modificar(g);
    }

    public void Borrar(Prevision p)
    {
        p.Estado = false;
        Modificar(p);
    }

    public void Guardar(string nombreUsuario)
    {
        try
        {
            StreamWriter fichero = File.CreateText(nombreUsuario + ".dat");
            foreach (Gasto g in gastos)
            {
                fichero.WriteLine(g.PrepararParaGuardar());
            }

            foreach (Prevision p in previsiones)
            {
                fichero.WriteLine(p.PrepararParaGuardar());
            }

            fichero.Close();
        }

        catch (IOException)
        {
            Console.WriteLine("Error de escritura");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }

    private void Cargar(string cadena)
    {
        string aux, fichero = cadena + ".dat";

        try
        {
            string[] datosDeFichero = File.ReadAllLines(fichero);

            foreach (string dato in datosDeFichero)
            {
                if (dato[0].ToString().Equals("G"))
                {
                    aux = dato.Substring(2);
                    Gasto g = new Gasto(aux);
                    gastos.Add(g);
                }

                if (dato[0].ToString().Equals("P"))
                {
                    aux = dato.Substring(2);
                    Prevision p = new Prevision(aux);
                    previsiones.Add(p);
                }
            }
        }
        catch (IOException)
        {
            Console.WriteLine("Error de lectura");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }

    public void OrdenarGastosPorFecha()
    {
        Gasto g;
        for (int i = 1; i < gastos.Count; i++)
            for (int j = gastos.Count - 1; j >= i; j--)
            {
                if (gastos[j - 1].Fecha > gastos[j].Fecha)
                {
                    g = gastos[j - 1];
                    gastos[j - 1] = gastos[j];
                    gastos[j] = g;
                }
            }
    }

    public void OrdenarPrevisionesPorFecha()
    {
        Prevision p;
        for (int i = 1; i < previsiones.Count; i++)
            for (int j = previsiones.Count - 1; j >= i; j--)
            {
                if (previsiones[j - 1].Fecha > previsiones[j].Fecha)
                {
                    p = previsiones[j - 1];
                    previsiones[j - 1] = previsiones[j];
                    previsiones[j] = p;
                }
            }
    }

    public void OrdenarGastosPorCategoria()
    {
        Gasto g;
        for (int i = 1; i < gastos.Count; i++)
            for (int j = gastos.Count - 1; j >= i; j--)
            {
                if (gastos[j - 1].Categoria[0] > gastos[j].Categoria[0])
                {
                    g = gastos[j - 1];
                    gastos[j - 1] = gastos[j];
                    gastos[j] = g;
                }
            }
    }

    public void OrdenarPrevisionesPorCategoria()
    {
        Prevision p;
        for (int i = 1; i < previsiones.Count; i++)
            for (int j = previsiones.Count - 1; j >= i; j--)
            {
                if (previsiones[j - 1].Categoria[0] > previsiones[j].Categoria[0])
                {
                    p = previsiones[j - 1];
                    previsiones[j - 1] = previsiones[j];
                    previsiones[j] = p;
                }
            }
    }

    public float GetMediaGastos(string categoria)
    {
        float media = 0;
        float suma = 0;
        int contador = 0;

        foreach (Gasto g in gastos)
        {
            if(g.Categoria.Equals(categoria))
            {
                suma += g.Importe;
                contador++;
            }
        }
        
        if (contador > 0)
            media = suma / contador;

        return media;
    }
}
