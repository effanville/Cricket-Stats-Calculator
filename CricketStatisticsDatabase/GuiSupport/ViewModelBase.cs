using Cricket.Interfaces;

namespace GUISupport.ViewModels
{
    /// <summary>
    /// Base for ViewModels containing display purpose objects.
    /// </summary>
    public abstract class ViewModelBase : PropertyChangedBase
    {
        public virtual string Header { get; }
        public virtual bool Closable { get { return false; } }

        public ViewModelBase(string header)
        {
            Header = header;
        }

        public abstract void UpdateData(ICricketTeam portfolio);
    }
}
