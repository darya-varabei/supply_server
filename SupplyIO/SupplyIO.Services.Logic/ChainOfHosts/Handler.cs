using SupplyIO.SupplyIO.Services.Models.CertificateModel;

namespace SupplyIO.SupplyIO.Services.Logic.ChainOfHosts
{
    public abstract class Handler
    {
        public Handler Successor { get; set; }
        public abstract Task<Certificate> HandleRequestAsync(Uri link);
    }
}
