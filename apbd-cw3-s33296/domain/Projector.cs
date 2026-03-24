namespace apbd_cw3_s33296.domain;

public class Projector : Equipment
{
    public string Resolution { get; }
    public int Lumens { get; }

    public Projector(int id, string name, string producent, string resolution, int lumens) : base(id, name,
        producent)
    {
        Resolution = resolution;
        Lumens = lumens;
    }

    protected override string GetSpecificDetails() => $"Rozdzielczość: {Resolution} | Jasność: {Lumens} lm";
}