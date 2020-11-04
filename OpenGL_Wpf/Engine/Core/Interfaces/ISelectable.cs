namespace Simple_Engine.Engine.Core.Interfaces
{
    public interface ISelectable : IDrawable
    {
        void Set_Selected(bool value);

        bool GetSelected();
    }
}