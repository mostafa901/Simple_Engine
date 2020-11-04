using Simple_Engine.Engine.Opticals;

namespace Simple_Engine.Engine.Core.Interfaces
{
    public interface IMaterial : IRenderable
    {
        public Gloss Glossiness { get; set; }
    }
}