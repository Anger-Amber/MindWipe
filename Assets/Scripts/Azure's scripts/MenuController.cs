using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject blackscreen;
    [SerializeField] GameObject confirmText;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Mouse currentMouse;
    [SerializeField] Camera mainCamera;
    [SerializeField] Vector3 oldPos;
    [SerializeField] Vector3 newPos;
    [SerializeField] Vector3 goalPos;
    [SerializeField] Vector3 lerpPos;
    [SerializeField] Vector3 oldColour;
    [SerializeField] Vector3 newColour;
    [SerializeField] Vector3 goalColour;
    [SerializeField] Vector3 lerpColour;
    [SerializeField] Vector3 originalScale;
    [SerializeField] Color tempColour;
    [SerializeField] Color uiMenuColorNew;
    [SerializeField] Color uiMenuColorOld;
    [SerializeField] float fadeSpeed;
    [SerializeField] float slideSpeed;
    public int musicVolume;
    [SerializeField] int menuButtonSelected;
    [SerializeField] bool startMenu;
    [SerializeField] bool settingsMenu;
    [SerializeField] bool quitMenu;
    [SerializeField] bool uiMenu;
    [SerializeField] bool isActive;
    [SerializeField] bool isFade;
    [SerializeField] bool confirmed;
    [SerializeField] bool clickedFromOtherScript;
    [SerializeField] bool settingOpen;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        currentMouse = Mouse.current;
        mainCamera = Camera.main;
    }

    private void OnMouseEnter()
    {
        if (!isFade)
        {
            goalColour = newColour;
        }
        if (uiMenu)
        {
            for (int i = 0; i <= 3; i++)
            {
                if (transform.parent.GetChild(i) == transform)
                {
                    transform.parent.parent.parent.GetChild(1).GetComponent<MenuController>().menuButtonSelected = i + 1;
                }
            }
        }
    }

    private void OnMouseExit()
    {
        if (!isFade)
        {
            goalColour = oldColour;
        }
        if (quitMenu)
        {
            goalPos = oldPos;
            confirmed = false;
        }
        if (uiMenu)
        {
            menuButtonSelected = transform.parent.parent.parent.GetChild(1).GetComponent<MenuController>().menuButtonSelected;
            menuButtonSelected--;
            if (menuButtonSelected >= 0 && menuButtonSelected <= 4 && transform.parent.GetChild(menuButtonSelected) == transform)
            {
                transform.parent.parent.parent.GetChild(1).GetComponent<MenuController>().menuButtonSelected = 0;
            }
            menuButtonSelected++;
        }
    }

    private void OnMouseDown()
    {
        OnClick();
    }
    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClick();
        }
    }

    private void FixedUpdate()
    {
        if (isActive)
        {
            lerpColour.x = tempColour.r * 255;
            lerpColour.y = tempColour.g * 255;
            lerpColour.z = tempColour.b * 255;
            lerpColour = Vector3.Lerp(goalColour, lerpColour, fadeSpeed);
            tempColour.r = lerpColour.x / 255;
            tempColour.g = lerpColour.y / 255;
            tempColour.b = lerpColour.z / 255;
            spriteRenderer.color = tempColour;

            transform.localScale = Vector3.Lerp(originalScale, transform.localScale, fadeSpeed);

            if (quitMenu || settingsMenu)
            {
                lerpPos = Vector3.Lerp(goalPos, lerpPos, slideSpeed);
                confirmText.transform.position = lerpPos * transform.parent.localScale.y;
            }
        }
        if (isFade)
        {
            if (lerpColour.z >= 250)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && startMenu && transform.parent.GetChild(1).GetComponent<MenuController>().settingOpen == false)
        {
            currentMouse.WarpCursorPosition(mainCamera.WorldToScreenPoint(transform.position));
        }
        if (Input.GetKeyDown(KeyCode.S) && settingsMenu && transform.parent.GetChild(1).GetComponent<MenuController>().settingOpen == false)
        {
            currentMouse.WarpCursorPosition(mainCamera.WorldToScreenPoint(transform.position));
        }
        if (Input.GetKeyDown(KeyCode.D) && quitMenu && transform.parent.GetChild(1).GetComponent<MenuController>().settingOpen == false)
        {
            currentMouse.WarpCursorPosition(mainCamera.WorldToScreenPoint(transform.position));
        }
        if (Input.GetKeyDown(KeyCode.W) && transform.parent.GetChild(1).GetComponent<MenuController>().settingOpen == true && transform.parent.GetChild(1) == transform && settingsMenu)
        {
            if (menuButtonSelected == 0)
            {
                menuButtonSelected = 1;
            }
            else if (menuButtonSelected == 1)
            {
                menuButtonSelected = 4;
            }
            else
            {
                menuButtonSelected--;
            }
            menuButtonSelected--;
            currentMouse.WarpCursorPosition
                (mainCamera.WorldToScreenPoint(transform.parent.GetChild(5).GetChild(0).GetChild
                (menuButtonSelected).transform.position));
            menuButtonSelected++;
        }
        if (Input.GetKeyDown(KeyCode.S) && transform.parent.GetChild(1).GetComponent<MenuController>().settingOpen == true && transform.parent.GetChild(1) == transform && settingsMenu)
        {
            if (menuButtonSelected == 0)
            {
                menuButtonSelected = 4;
            }
            else if (menuButtonSelected == 4)
            {
                menuButtonSelected = 1;
            }
            else
            {
                menuButtonSelected++;
            }
            menuButtonSelected--;
            currentMouse.WarpCursorPosition
                (mainCamera.WorldToScreenPoint(transform.parent.GetChild(5).GetChild(0).GetChild
                (menuButtonSelected).transform.position));
            menuButtonSelected++;
        }
    }

    private void OnClick()
    {
        if (!isFade)
        {
            if (!clickedFromOtherScript)
            {
                transform.localScale *= 0.9f;
            }
            else
            {
                clickedFromOtherScript = false;
            }
            if (startMenu)
            {
                blackscreen.GetComponent<MenuController>().isActive = true;
                blackscreen.GetComponent<MenuController>().isFade = true;
            }
            if (quitMenu)
            {
                if (confirmed)
                {
                    Debug.Log("quit");
                    Application.Quit();
                }
                if (!confirmed)
                {
                    confirmed = true;
                    goalPos = newPos;
                }
            }
            if (settingsMenu)
            {
                if (transform.parent.parent == null)
                {
                    if (settingOpen)
                    {
                        settingOpen = false;
                        goalPos = oldPos;
                    }
                    else if (!settingOpen)
                    {
                        settingOpen = true;
                        goalPos = newPos;
                    }
                }
                else
                {
                    transform.parent.parent.parent.GetChild(1).GetComponent<MenuController>().clickedFromOtherScript = true;
                    transform.parent.parent.parent.GetChild(1).GetComponent<MenuController>().OnClick();
                }
            }
            if (uiMenu && !settingsMenu)
            {
                if (musicVolume >= 1)
                {
                    musicVolume--;
                }
                else
                {
                    musicVolume = 9;
                }
                for (int i = 9; i > musicVolume; i--)
                {
                    transform.GetChild(i).GetComponent<SpriteRenderer>().color = uiMenuColorNew;
                }
                for (int i = 1; i <= musicVolume; i++)
                {
                    transform.GetChild(i).GetComponent<SpriteRenderer>().color = uiMenuColorOld;
                }
            }
        }
    }
}