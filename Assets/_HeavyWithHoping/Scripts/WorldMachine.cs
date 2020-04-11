using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WorldMachine : MonoBehaviour
{
    [SerializeField]
    private GameObject plane = null;

    [SerializeField]
    private FlexibleColorPicker fcp;

    [SerializeField]
    private Material mat;

    [SerializeField]
    private Slider speedSlider;

    [SerializeField]
    private Color colorOne = Color.white;
    [SerializeField]
    private Color colorTwo = Color.white;
    [SerializeField]
    private Color colorThree = Color.white;
    [SerializeField]
    private Color colorFour = Color.white;

    [SerializeField]
    private Image colorOneUI;
    [SerializeField]
    private Image colorTwoUI;
    [SerializeField]
    private Image colorThreeUI;
    [SerializeField]
    private Image colorFourUI;

    [SerializeField]
    private GameObject editor;

    [SerializeField]
    private bool isEditorOpen;

    [SerializeField]
    private int selectedColor;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private GameObject quitMenu;

    private void Start()
    {
        float val = Camera.main.pixelHeight / Camera.main.pixelWidth / 2.0f;
        plane.transform.localScale = new Vector3(val, 1, 1);

        colorOne = mat.GetColor("_ColorOne");
        colorTwo = mat.GetColor("_ColorTwo");
        colorThree = mat.GetColor("_ColorThree");
        colorFour = mat.GetColor("_ColorFour");

        colorOneUI.color = colorOne;
        colorTwoUI.color = colorTwo;
        colorThreeUI.color = colorThree;
        colorFourUI.color = colorFour;

        fcp.color = colorOne;

        isEditorOpen = false;
        selectedColor = 1;

        editor.transform.localPosition = new Vector3(0, 2000, 0);

        speedSlider.value = mat.GetFloat("_Speed");

        quitMenu.SetActive(false);
    }

    void Update()
    {
        //Debug.Log($"Height: {Screen.height }");
        //Debug.Log($"Width: {Screen.width }");
        float val = (float)Screen.width / (float)Screen.height;
        plane.transform.localScale = new Vector3(val, 1, 1);

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            StopAllCoroutines();
            StartCoroutine(LerpEditor(false));

            quitMenu.SetActive(!quitMenu.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (quitMenu.activeSelf == true)
            {

            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(LerpEditor(!isEditorOpen));
            }
        }


        if (IsDoubleTap() == true)
        {
            StopAllCoroutines();
            StartCoroutine(LerpEditor(!isEditorOpen));
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            audio.volume += 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            audio.volume -= 0.1f;
        }

        if (isEditorOpen == true)
        {
            mat.SetFloat("_Speed", speedSlider.value);

            if (selectedColor == 1)
            {
                colorOne = fcp.color;
                colorOneUI.color = colorOne;
                mat.SetColor("_ColorOne", colorOne);
            }
            else if (selectedColor == 2)
            {
                colorTwo = fcp.color;
                colorTwoUI.color = colorTwo;
                mat.SetColor("_ColorTwo", colorTwo);
            }
            else if (selectedColor == 3)
            {
                colorThree = fcp.color;
                colorThreeUI.color = colorThree;
                mat.SetColor("_ColorThree", colorThree);
            }
            else if (selectedColor == 4)
            {
                colorFour = fcp.color;
                colorFourUI.color = colorFour;
                mat.SetColor("_ColorFour", colorFour);
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (audio.isPlaying == true)
            {
                audio.Pause();
            }
            else
            {
                audio.Play();
            }
        }
    }

    public void OnButtonClick(int idx)
    {
        selectedColor = idx;
        //i hate myself for not using an array but quick and dirty
        switch (idx)
        {
            case 1:
                fcp.color = colorOne;
                break;
            case 2:
                fcp.color = colorTwo;
                break;
            case 3:
                fcp.color = colorThree;
                break;
            case 4:
                fcp.color = colorFour;
                break;
        }
    }

    private IEnumerator LerpEditor(bool open)
    {
        float time = 0;
        Vector3 startPos = editor.transform.localPosition;

        isEditorOpen = open;

        while (true)
        {
            if (open == true)
            {
                editor.transform.localPosition += (new Vector3(0, 0, 0) - startPos) * Time.deltaTime * 3.0f;

                if (editor.transform.localPosition.y < 0)
                {
                    editor.transform.localPosition = new Vector3(0, 0, 0);
                    break;
                }
            }
            else
            {
                editor.transform.localPosition += (new Vector3(0, 2000, 0) - startPos) * Time.deltaTime * 3.0f;

                if (editor.transform.localPosition.y > 2000)
                {
                    editor.transform.localPosition = new Vector3(0, 2000, 0);
                    break;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private static bool IsDoubleTap()
    {
        bool result = false;
        float MaxTimeWait = 0.25f;
        float VariancePosition = 1;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            float DeltaTime = Input.GetTouch(0).deltaTime;
            float DeltaPositionLenght = Input.GetTouch(0).deltaPosition.magnitude;

            if (DeltaTime > 0 && DeltaTime < MaxTimeWait && DeltaPositionLenght < VariancePosition)
                result = true;
        }
        return result;
    }

    public void OnQuitButton()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnNoQuitButton()
    {
        quitMenu.SetActive(false);
    }

}
