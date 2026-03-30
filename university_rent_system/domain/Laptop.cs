namespace apbd_cw3_s33296.domain;

public class Laptop : Equipment
{
    public string Processor { get; }
    public int Ram { get; set; }
    public bool HasGpu { get; }
    
    public Laptop(int id, string name, string producent, string processor, int ram, bool hasGpu) : base(id, name, producent)
    {
        Processor = processor;
        Ram = ram;
        HasGpu = hasGpu;
    }
    
    protected override string GetSpecificDetails() => $"RAM: {Ram} GB | Dedykowane GPU: {(HasGpu ? "Tak" : "Nie")}";
}
