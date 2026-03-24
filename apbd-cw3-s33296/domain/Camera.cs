namespace apbd_cw3_s33296.domain;

public class Camera : Equipment
{
    public int Megapixels { get; }
    public string LensMount { get; }

    public Camera(string id, string name, string producent, int megapixels, string lensMount) : base(id, name, producent)
    {
        Megapixels = megapixels;
        LensMount = lensMount;
    }

    protected override string GetSpecificDetails()
        => $"MP: {Megapixels} | Mocowanie obiektywu: {LensMount}";
}