using System;
using System.Collections.Generic;
using System.IO;

class GestionUsuarios
{
    Usuario[] usuarios;

    public GestionUsuarios()
    {
        usuarios = new Usuario[4];
        usuarios = Cargar();
    }

    public Usuario[] GetUsuarios()
    {
        return usuarios;
    }

    private Usuario[] Cargar()
    {
        Usuario[] usuarios = InicializarUsuarios();
        Usuario nuevoUsuario;
        int usuariosCargados = 0;
        string aux = "";

        if (!File.Exists("conf.dat"))
        {
            return usuarios;
        }

        try
        {
            string[] datosDeFichero = File.ReadAllLines("conf.dat");

            foreach (string dato in datosDeFichero)
            {
                if (dato[0].ToString().Equals("U"))
                {
                    aux = dato.Substring(2);
                    nuevoUsuario = new Usuario(aux);
                    usuarios[usuariosCargados] = nuevoUsuario;
                    usuariosCargados++;
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

        return usuarios;
    }

    public void Guardar()
    {
        List<string> datos = new List<string>();
        try
        {
            StreamWriter fichero = File.CreateText("conf.dat");
            foreach (Usuario usuario in usuarios)
            {
                if (usuario.GetSobrenombre() != "Nuevo Usuario.")
                    fichero.WriteLine(usuario.PrepararParaGuardar());
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

    private Usuario[] InicializarUsuarios()
    {
        Usuario[] usuarios = new Usuario[4];
        for (int i = 0; i < 4; i++)
        {
            usuarios[i] = new Usuario();
        }
        return usuarios;

    }

    public void InsertarUsuario(Usuario usuario, int posUsuario)
    {
        usuarios[posUsuario] = usuario;
        Guardar();
    }
}

