using System;

namespace Core.models
{
    public class RouteMeasures
    {
        public double Distance { get; set; }
        public int Jumps { get; set;}
        public EPathStatus Status { get; }
        public int QuantityOfExpansions { get; set; }
        public TimeSpan DeltaTime { get; set; }
        public long SourceId {get; set;}
        public long TargetId {get; set;}
        public string RouteKey { get { return SourceId.ToString() + Constants.SEPARATOR_ODs + TargetId.ToString(); }}
        
        public RouteMeasures(EPathStatus Status)
        {
            this.Status = Status;
        }
    }
}