namespace Half_Measure.UI 
{
    public abstract class UIBase : AutoMonoBehaviour
    {
        public virtual void Open() => gameObject.SetActive(true);

        public virtual void Close() => gameObject.SetActive(false);
    }
}
