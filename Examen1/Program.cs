using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static List<Paciente> pacientes = new List<Paciente>();
    static List<Medicamento> catalogoMedicamentos = new List<Medicamento>();
    static List<Consulta> consultas = new List<Consulta>();

    static void Main(string[] args)
    {
        int opcion;
        do
        {
            Console.WriteLine("Menú principal:");
            Console.WriteLine("1- Agregar paciente");
            Console.WriteLine("2- Agregar medicamento al catálogo");
            Console.WriteLine("3- Asignar tratamiento médico a un paciente");
            Console.WriteLine("4- Consultas");
            Console.WriteLine("5- Salir");
            Console.Write("Seleccione una opción: ");
            if (int.TryParse(Console.ReadLine(), out opcion))
            {
                switch (opcion)
                {
                    case 1:
                        AgregarPaciente();
                        break;
                    case 2:
                        AgregarMedicamentoCatalogo();
                        break;
                    case 3:
                        AsignarTratamientoMedico();
                        break;
                    case 4:
                        RealizarConsultas();
                        break;
                    case 5:
                        Console.WriteLine("Saliendo del programa...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Por favor, seleccione una opción válida.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Entrada no válida. Por favor, ingrese un número válido.");
            }
        } while (opcion != 5);
    }

    static void AgregarPaciente()
    {
        Paciente paciente = new Paciente();

        Console.WriteLine("Ingrese los datos del paciente:");
        Console.Write("Nombre: ");
        paciente.Nombre = Console.ReadLine();
        Console.Write("Cedula: ");
        paciente.Cedula = Console.ReadLine();
        Console.Write("Teléfono: ");
        paciente.Telefono = Console.ReadLine();
        Console.Write("Tipo de sangre: ");
        paciente.TipoSangre = Console.ReadLine();
        Console.Write("Dirección: ");
        paciente.Direccion = Console.ReadLine();
        Console.Write("Fecha de Nacimiento (Año-Mes-Dia): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime fechaNacimiento))
        {
            paciente.FechaNacimiento = fechaNacimiento;
        }
        else
        {
            Console.WriteLine("Formato de fecha inválido. Se asignará la fecha actual.");
            paciente.FechaNacimiento = DateTime.Now;
        }

        pacientes.Add(paciente);
            }

    static void AgregarMedicamentoCatalogo()
    {
        Medicamento medicamento = new Medicamento();

        Console.WriteLine("Ingrese los datos del medicamento:");
        Console.Write("Código del medicamento: ");
        medicamento.Codigo = Console.ReadLine();
        Console.Write("Nombre del medicamento: ");
        medicamento.Nombre = Console.ReadLine();
        Console.Write("Cantidad: ");
        if (int.TryParse(Console.ReadLine(), out int cantidad) && cantidad >= 0)
        {
            medicamento.Cantidad = cantidad;
        }
        else
        {
            Console.WriteLine("Cantidad inválida. Se asignará 0.");
            medicamento.Cantidad = 0;
        }

        catalogoMedicamentos.Add(medicamento);
        Console.WriteLine("Medicamento agregado correctamente al catálogo.");
    }

    static void AsignarTratamientoMedico()
    {
        if (pacientes.Count == 0)
        {
            Console.WriteLine("No hay pacientes registrados.");
            return;
        }

        Console.WriteLine("Seleccione el paciente al que desea asignar tratamiento:");
        for (int i = 0; i < pacientes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {pacientes[i].Nombre} - {pacientes[i].Cedula}");
        }
        Console.Write("Seleccione un paciente (1-{0}): ", pacientes.Count);
        if (int.TryParse(Console.ReadLine(), out int indicePaciente) && indicePaciente >= 1 && indicePaciente <= pacientes.Count)
        {
            Paciente paciente = pacientes[indicePaciente - 1];

            Console.WriteLine("Seleccione el medicamento a asignar:");
            for (int i = 0; i < catalogoMedicamentos.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {catalogoMedicamentos[i].Nombre} - Cantidad: {catalogoMedicamentos[i].Cantidad}");
            }

            Console.Write("Seleccione un medicamento (1-{0}): ", catalogoMedicamentos.Count);
            if (int.TryParse(Console.ReadLine(), out int indiceMedicamento) && indiceMedicamento >= 1 && indiceMedicamento <= catalogoMedicamentos.Count)
            {
                Medicamento medicamento = catalogoMedicamentos[indiceMedicamento - 1];

                if (medicamento.Cantidad > 0)
                {
                    medicamento.Cantidad--;
                    Consulta consulta = new Consulta { Paciente = paciente, Medicamento = medicamento };
                    consultas.Add(consulta);
                    Console.WriteLine("Tratamiento asignado correctamente.");
                }
                else
                {
                    Console.WriteLine("El medicamento seleccionado no está disponible.");
                }
            }
            else
            {
                Console.WriteLine("Selección de medicamento inválida.");
            }
        }
        else
        {
            Console.WriteLine("Selección de paciente inválida.");
        }
    }

    static void RealizarConsultas()
    {
        Console.WriteLine($"Cantidad total de pacientes registrados: {pacientes.Count}");

        Console.WriteLine("Reporte de todos los medicamentos recetados:");
        var medicamentosUnicos = consultas.Select(c => c.Medicamento).Distinct();
        foreach (var medicamento in medicamentosUnicos)
        {
            Console.WriteLine($"- {medicamento.Nombre}");
        }

        Console.WriteLine("Reporte de cantidad de pacientes agrupados por edades:");
        int Menor10 = pacientes.Count(p => (DateTime.Now - p.FechaNacimiento).TotalDays / 365 <= 10);
        int de11a30 = pacientes.Count(p => (DateTime.Now - p.FechaNacimiento).TotalDays / 365 > 10 && (DateTime.Now - p.FechaNacimiento).TotalDays / 365 <= 30);
        int de31a50 = pacientes.Count(p => (DateTime.Now - p.FechaNacimiento).TotalDays / 365 > 30 && (DateTime.Now - p.FechaNacimiento).TotalDays / 365 <= 50);
        int Mayor50 = pacientes.Count(p => (DateTime.Now - p.FechaNacimiento).TotalDays / 365 > 50);
        Console.WriteLine($"- 0-10 años: {Menor10}");
        Console.WriteLine($"- 11-30 años: {de11a30}");
        Console.WriteLine($"- 31-50 años: {de31a50}");
        Console.WriteLine($"- Mayores de 51 años: {Mayor50}");

        Console.WriteLine("Reporte de pacientes y consultas ordenado por nombre:");
        var pacientesOrdenados = pacientes.OrderBy(p => p.Nombre);
        foreach (var paciente in pacientesOrdenados)
        {
            Console.WriteLine($"- {paciente.Nombre} ({paciente.Cedula})");
            var consultasPaciente = consultas.Where(c => c.Paciente == paciente);
            foreach (var consulta in consultasPaciente)
            {
                Console.WriteLine($"  * Tratamiento: {consulta.Medicamento.Nombre}");
            }
        }

        Console.WriteLine("Búsqueda de paciente por número de cédula:");
        Console.Write("Ingrese el número de cédula a buscar: ");
        string cedulaBuscar = Console.ReadLine();
        var pacienteEncontrado = pacientes.FirstOrDefault(p => p.Cedula == cedulaBuscar);
        if (pacienteEncontrado != null)
        {
            Console.WriteLine($"Paciente encontrado: {pacienteEncontrado.Nombre}");
        }
        else
        {
            Console.WriteLine("No se encontró ningún paciente con ese número de cédula.");
        }
    }
}

class Paciente
{
    public string Nombre { get; set; }
    public string Cedula { get; set; }
    public string Telefono { get; set; }
    public string TipoSangre { get; set; }
    public string Direccion { get; set; }
    public DateTime FechaNacimiento { get; set; }
}

class Medicamento
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public int Cantidad { get; set; }
}

class Consulta
{
    public Paciente Paciente { get; set; }
    public Medicamento Medicamento { get; set; }
}

