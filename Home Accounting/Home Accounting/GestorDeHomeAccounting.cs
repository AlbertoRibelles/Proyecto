using System;
using System.Collections.Generic;

public class GestorDeHomeAccounting
{
    Pantalla pan = new Pantalla();
    ListaDeGastosYPrevisiones lista;
    List<Gasto> gastosAExportar = new List<Gasto>();
    ExportacionDatos exportador = new ExportacionDatos();
    DateTime fechaVisualizar = DateTime.Now;
    Gasto gastoAux;
    Prevision previsionAux;
    int posicion = 1, inicioPagina = 1, finPagina = 10, opcion = 0;
    string datos, nombreFichero, conceptoABuscar = "";
    ConsoleKeyInfo entrada;
    bool terminado = false;
    bool modoVisualizacion = false;
    bool ordenacionPorFecha = true;
    bool visualizacionGastos = true;

    GestionUsuarios gUsuarios = new GestionUsuarios();
    Usuario uActual;
    bool finMenuUsuario = false;
    bool iniciar = true;
    int usuarioSeleccionado = 0;
    
    public GestorDeHomeAccounting()
    {
        Console.BackgroundColor = ConsoleColor.DarkBlue;
    }
    
    private void MenuUsuario()
    {
        do
        {
            pan.DibujarPantallaBienvenida();
            uActual = pan.DibujarMenuUsuarios(usuarioSeleccionado, 
                gUsuarios.GetUsuarios());
            entrada = Console.ReadKey();

            switch (entrada.Key)
            {
                case ConsoleKey.Enter:
                    finMenuUsuario = true;
                    if (uActual.GetSobrenombre() == "Nuevo Usuario.")
                    {
                        uActual = pan.DibujarCreacionUsuarios();
                        gUsuarios.InsertarUsuario(uActual, usuarioSeleccionado);
                    }
                    break;
                case ConsoleKey.Escape:
                    iniciar = false;
                    finMenuUsuario = true;
                    Console.ResetColor();
                    Console.Clear();
                    break;
                case ConsoleKey.UpArrow:
                    if (usuarioSeleccionado > 0)
                        usuarioSeleccionado--;
                    break;

                case ConsoleKey.DownArrow:
                    if (usuarioSeleccionado < 3)
                        usuarioSeleccionado++;
                    break;
            }
        } while (!finMenuUsuario);
    }

    private void MenuGastos()
    {
        if (iniciar)
        {
            lista = new ListaDeGastosYPrevisiones(uActual.GetSobrenombre());

            do
            {
                pan.DibujarPantalla();
                if (visualizacionGastos)
                {
                    gastoAux = pan.DibujarGastosMensuales(modoVisualizacion, 
                        lista, fechaVisualizar, posicion, inicioPagina, 
                        finPagina, conceptoABuscar, ordenacionPorFecha);
                }
                else
                {
                    previsionAux = pan.DibujarPrevisionesMensuales(
                        modoVisualizacion, lista, fechaVisualizar, posicion, 
                        inicioPagina, finPagina, conceptoABuscar, 
                        ordenacionPorFecha);
                }
                Console.SetCursorPosition(118, 2);
                entrada = Console.ReadKey();

                switch (entrada.Key)
                {
                    case ConsoleKey.F1: Anyadir(); break;
                    case ConsoleKey.F2: Modificar(); break;
                    case ConsoleKey.F3: VerDetalle(); break;
                    case ConsoleKey.F4: VerDetalleDescripcion(); break;
                    case ConsoleKey.F5: Borrar(); break;
                    case ConsoleKey.F6: Configuracion(); break;
                    case ConsoleKey.F7: ExportarExcelCsv(); break;
                    case ConsoleKey.F8: ExportarPDF(); break;
                    case ConsoleKey.F9: break;
                    case ConsoleKey.F10: Agrupar(); break;
                    case ConsoleKey.RightArrow: NavegarMeses(entrada.Key); break;
                    case ConsoleKey.LeftArrow: NavegarMeses(entrada.Key); break;
                    case ConsoleKey.UpArrow: 
                        MoverVertical(entrada.Key); break;
                    case ConsoleKey.DownArrow: 
                        MoverVertical(entrada.Key); break;
                    case ConsoleKey.F: ResetearValoresPagina(); break;
                    case ConsoleKey.S: Salir(); break;
                }
            } while (!terminado);
        }
        Console.WriteLine("Gracias por usar AR Software");
    }

    private void ResetearValoresPagina()
    {
        conceptoABuscar = "";
        posicion = 1;
        inicioPagina = 1;
        finPagina = 10;
    }

    private void Anyadir()
    {
        datos = pan.DibujarMenuAnyadir(lista.GetProximoIDGasto());

        if (visualizacionGastos)
            lista.Anyadir(new Gasto(datos));
        else
            lista.Anyadir(new Prevision(datos));

        lista.Guardar(uActual.GetSobrenombre());
    }

    private void Modificar()
    {
        posicion = pan.DibujarPeticionGasto(modoVisualizacion, 
            visualizacionGastos, lista, fechaVisualizar, ordenacionPorFecha);
        if (visualizacionGastos)
        {
            gastoAux = pan.DibujarGastosMensuales(modoVisualizacion, lista, 
                fechaVisualizar, posicion, inicioPagina, finPagina,
                conceptoABuscar, ordenacionPorFecha);

            lista.Modificar(new Gasto(pan.DibujarMenuModificar(
                gastoAux.ToDato())));
        }
        else
        {
            previsionAux = pan.DibujarPrevisionesMensuales(modoVisualizacion,
            lista, fechaVisualizar, posicion, inicioPagina, finPagina,
            conceptoABuscar, ordenacionPorFecha);

            lista.Modificar(new Prevision(pan.DibujarMenuModificar(
                previsionAux.ToDato())));
        }

        lista.Guardar(uActual.GetSobrenombre());
    }

    private void VerDetalle()
    {
        posicion = pan.DibujarPeticionGasto(modoVisualizacion, 
            visualizacionGastos,lista, fechaVisualizar, ordenacionPorFecha);
    }

    private void VerDetalleDescripcion()
    {
        conceptoABuscar = "";
        posicion = 1;
        inicioPagina = 1;
        finPagina = 10;
        conceptoABuscar = pan.DibujarPeticionConcepto();
    }

    private void Borrar()
    {
        posicion = pan.DibujarPeticionGasto(modoVisualizacion, 
            visualizacionGastos, lista, fechaVisualizar, ordenacionPorFecha);
        gastoAux = pan.DibujarGastosMensuales(modoVisualizacion, lista, 
            fechaVisualizar, posicion, inicioPagina, finPagina, conceptoABuscar,
            ordenacionPorFecha);
        lista.Borrar(gastoAux);
        posicion = 1;
        lista.Guardar(uActual.GetSobrenombre());
    }
    private bool Configuracion()
    {
        modoVisualizacion = pan.DibujarMenuConfiguracion(modoVisualizacion);

        return modoVisualizacion;
    }

    private void ExportarExcelCsv()
    {
        opcion = pan.DibujarMenuExportar(
            "Pulse '1' para exportar en formato Excel.", 
            "Pulse '2' para exportar en formato Csv.");

        nombreFichero = pan.DibujarPeticionNombreFichero();

        if (opcion == 1)
            exportador.GenerarExcel(lista.GetGastos(), nombreFichero);

        if (opcion == 2)
            exportador.GenerarCsv(lista.GetGastos(), nombreFichero);
    }

    private void ExportarPDF()
    {
        opcion = pan.DibujarMenuExportar("Pulse '1' para exportar todos los " +
            "gastos.", "Pulse '2' para exportar los gastos del mes actual.");
        nombreFichero = pan.DibujarPeticionNombreFichero();

        if (opcion == 1)
        {
            gastosAExportar = lista.GetGastos();
            exportador.GenerarPDFGTotales(gastosAExportar, nombreFichero);
        }
            

        if (opcion == 2)
        {
            gastosAExportar = lista.GetGastos(modoVisualizacion,
                ordenacionPorFecha, fechaVisualizar);
            exportador.GenerarPDFGMensuales(gastosAExportar, nombreFichero);
        }
            
        
    }

    private bool Agrupar()
    {
        if (ordenacionPorFecha)
            ordenacionPorFecha = false;
        else
            ordenacionPorFecha = true;

        posicion = 1;

        return ordenacionPorFecha;
    }

    private DateTime NavegarMeses(ConsoleKey flecha)
    {
        if (flecha == ConsoleKey.RightArrow)
        {
            posicion = 1;
            inicioPagina = 1;
            finPagina = 10;
            fechaVisualizar = fechaVisualizar.AddMonths(1);
        }

        if(flecha == ConsoleKey.LeftArrow)
        {

            posicion = 1;
            inicioPagina = 1;
            finPagina = 10;
            fechaVisualizar = fechaVisualizar.AddMonths(-1);
        }

        return fechaVisualizar;
    }

    private bool Salir()
    {
        lista.Guardar(uActual.GetSobrenombre());
        Console.ResetColor();
        Console.Clear();
        terminado = true;

        return terminado;
    }

    private void MoverVertical(ConsoleKey flecha)
    {
        if (flecha == ConsoleKey.UpArrow)
        {
            if (posicion > 1)
                posicion--;
            if (posicion < inicioPagina)
            {
                inicioPagina -= 10;
                finPagina -= 10;
            }
        }

        if (flecha == ConsoleKey.DownArrow)
        {
            if (posicion < lista.GetGastos(modoVisualizacion,
                            ordenacionPorFecha, fechaVisualizar).Count)
                posicion++;
            if (posicion > finPagina)
            {
                inicioPagina += 10;
                finPagina += 10;
            }
        }
    }

    public void Ejecutar()
    {
        MenuUsuario();
        MenuGastos();
    }
}
