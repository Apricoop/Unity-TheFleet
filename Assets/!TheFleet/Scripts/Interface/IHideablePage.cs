public interface IHideablePage
{
    void Hide(float duration = 0.2f);
    void Show(float duration = 0.2f);
    bool IsHidden { get; set; }
}

