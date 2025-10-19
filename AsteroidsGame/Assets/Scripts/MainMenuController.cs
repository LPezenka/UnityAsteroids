using UnityEngine;
using UnityEngine.UIElements;
public class MainMenuController : MonoBehaviour
{
    public UIDocument mainMenu;
    public GameObject gameController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        mainMenu = GetComponent<UIDocument>();
        var root = mainMenu.rootVisualElement;
        var newGameButton = root.Q<Button>("ButtonNewGame");
        var quitGameButton = root.Q<Button>("ButtonQuitGame");

        newGameButton.clicked += () => gameController.GetComponent<GameController>().StartGame();
        quitGameButton.clicked += () => Application.Quit();
    }

    private void OnDisable()
    {
        //var root = mainMenu.rootVisualElement;
        //var newGameButton = root.Q<Button>("ButtonNewGame");
        //var quitGameButton = root.Q<Button>("ButtonQuitGame");

        //newGameButton.clicked -= () => gameController.GetComponent<GameController>().StartGame();
        //quitGameButton.clicked -= () => Application.Quit();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
