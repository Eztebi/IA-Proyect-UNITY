using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject menuPanel;

    public void OpenWinPanel()
    {
        RemoveAllPanels();
        winPanel.SetActive(true);

    }
    public void OpenLosePanel()
    {
        RemoveAllPanels();
        losePanel.SetActive(true);
    }
    public void OpenMenuPanel()
    {
        RemoveAllPanels();
        menuPanel.SetActive(true);
    }
    public void RemoveAllPanels()
    {
        losePanel.SetActive(false);
        menuPanel.SetActive(false);
        winPanel.SetActive(false);
    }
}
