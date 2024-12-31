using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Scripts : MonoBehaviour
{
    public GameObject BackGround_Panel;
    public Sprite Rounded_Square;
    public Sprite UISprite;

    private char[] first_group_of_letters = { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'Ð', 'Ü' };
    private char[] second_group_of_letters = { 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Þ', 'Ý' };
    private char[] third_group_of_letters = { 'Z', 'X', 'C', 'V', 'B', 'N', 'M', 'Ö', 'Ç' };

    private int current_row;
    private int current_column;

    private TextAsset textFile;
    private List<string> Word_List;

    public string Selected_Word;
    private string Typed_Word; // kullanýlan harfleri renklendirme

    public GameObject Score_Result_Text;
    private int score;
    private int guess_left;

    public GameObject GameOver_Panel;
    public Text Last_Score_Result_Text;

    public Color Choosen_Color(string colorname)
    {
        Color mycolor = Color.black;

        if (colorname == "Gold")
        {
            mycolor = new Color(1f, 215f / 255, 0f);
        }
        else if (colorname == "Correct Green")
        {
            mycolor = new Color(144f / 255, 198f / 255, 33f / 255);
        }
        else if (colorname == "Correct Cyan")
        {
            mycolor = new Color(0f, 180f / 255, 207f / 255);
        }

        return mycolor;
    }

    private void RectTransform_Values(RectTransform rec, float maxx, float maxy, float minx, float miny, int posX, int posY, int SizeX, int SizeY)
    {
        rec.localScale = Vector3.one;
        rec.anchorMax = new Vector2(maxx, maxy);
        rec.anchorMin = new Vector2(minx, miny);
        rec.anchoredPosition = new Vector2(posX, posY);
        rec.sizeDelta = new Vector2(SizeX, SizeY);
    }

    private void VerticalLayoutGroup_Values(VerticalLayoutGroup vlc, int Pl, int Pt, int Pb, int Pr, int Spacing, TextAnchor Ta) //child hizalama
    {
        vlc.padding.left = Pl;
        vlc.padding.right = Pr;
        vlc.padding.top = Pt;
        vlc.padding.bottom = Pb;
        vlc.spacing = Spacing;
        vlc.childAlignment = Ta;
        vlc.childControlHeight = false;
        vlc.childControlWidth = false;
        vlc.reverseArrangement = false;
        vlc.childScaleHeight = false;
        vlc.childScaleWidth = false;
        vlc.childForceExpandHeight = false;
        vlc.childForceExpandWidth = false;
    }

    private void HorizontalLayoutGroup_Values(HorizontalLayoutGroup hlc, int Pl, int Pt, int Pb, int Pr, int Spacing, TextAnchor Ta) //child hizalama
    {
        hlc.padding.left = Pl;
        hlc.padding.right = Pr;
        hlc.padding.top = Pt;
        hlc.padding.bottom = Pb;
        hlc.spacing = Spacing;
        hlc.childAlignment = Ta;
        hlc.childControlHeight = false;
        hlc.childControlWidth = false;
        hlc.reverseArrangement = false;
        hlc.childScaleHeight = false;
        hlc.childScaleWidth = false;
        hlc.childForceExpandHeight = false;
        hlc.childForceExpandWidth = false;
    }

    private void Create_All_Rows()
    {
        if (BackGround_Panel != null)
        {
            GameObject AllRows = new GameObject("All Rows");
            AllRows.transform.SetParent(BackGround_Panel.transform);
            RectTransform rec_4_AllRows = AllRows.AddComponent<RectTransform>();
            RectTransform_Values(rec_4_AllRows, 0.5f, 0.5f, 0.5f, 0.5f, 620, 520, 2365, 1000); // Orta hizalama, 4 ve 5. deðer satýr ve sütunlarý saða sola oynatma

            VerticalLayoutGroup vlc_4_AllRows = AllRows.AddComponent<VerticalLayoutGroup>();
            VerticalLayoutGroup_Values(vlc_4_AllRows, 0, 0, 200, 0, 100, TextAnchor.UpperLeft); // en saðdaki deðer satýr boþluðu

            for (int i = 0; i < 6; i++)
            {
                GameObject Rows = new GameObject("Rows");
                Rows.transform.SetParent(AllRows.transform);

                RectTransform rec_4_Rows = Rows.AddComponent<RectTransform>();
                RectTransform_Values(rec_4_Rows, 0.9f, 0.9f, 0.9f, 0.9f, -540, 1111, 150, 125); // Orta hizalama, boyutlar 

                HorizontalLayoutGroup hlc_4_Rows = Rows.AddComponent<HorizontalLayoutGroup>();
                HorizontalLayoutGroup_Values(hlc_4_Rows, 0, 0, -20, 0, -20, TextAnchor.UpperLeft); // Sütun boþluðunu artýrmak için 5. deðeri ayarla

                for (int j = 0; j < 5; j++)
                {
                    GameObject imgObject = new GameObject("Square");
                    imgObject.transform.SetParent(Rows.transform);

                    RectTransform rec_4_imgObject = imgObject.AddComponent<RectTransform>();
                    RectTransform_Values(rec_4_imgObject, 0, 1, 0, 1, 400 * j, 350, 240, 240); // Pos X deðeri ayarlanýyor, boyutlar son 2 deðer

                    Image img_rs = imgObject.AddComponent<Image>();
                    img_rs.sprite = Rounded_Square;

                    GameObject panel = new GameObject("panel_R" + i + "_C" + j);
                    panel.transform.SetParent(imgObject.transform);

                    RectTransform rec_4_panel = panel.AddComponent<RectTransform>();
                    RectTransform_Values(rec_4_panel, 0.5f, 0.5f, 0.5f, 0.5f, 0, 0, 175, 175);

                    Image img = panel.AddComponent<Image>();
                    img.color = BackGround_Panel.GetComponent<Image>().color;

                    GameObject textGui = new GameObject("Text_R" + i + "_C" + j);
                    textGui.transform.SetParent(panel.transform);

                    RectTransform rec_4_textGui = textGui.AddComponent<RectTransform>();
                    RectTransform_Values(rec_4_textGui, 1, 1, 0, 0, 0, 0, 0, 0);
                    CanvasRenderer Cr_4_textGui = textGui.AddComponent<CanvasRenderer>();
                    Cr_4_textGui.cullTransparentMesh = true;


                    Text Text = textGui.AddComponent<Text>();
                    Text.text = "";
                    Text.font = Font.CreateDynamicFontFromOSFont("Arial", 16);
                    Text.fontStyle = FontStyle.Bold;
                    Text.fontSize = 100;
                    Text.alignment = TextAnchor.MiddleCenter;
                    Text.color = Color.black;

                }
            }
        }
        else
        {
            Debug.LogError("BackGround_Panel is not assigned!");
        }
    }

    /*private void Submit_Button_Changes(Color newcolor, string buttontext, int fontsize) //submit buton sürekli renk deðiþtirmemesi için oluþturdupum fonk
    {
        GameObject Submit_Button = GameObject.Find("Submit_Button");
        var colors = Submit_Button.GetComponent<Button>().colors;
        colors.normalColor = newcolor;
        Submit_Button.GetComponent<Button>().colors = colors;

        GameObject textgui = GameObject.Find("Submit_Button_Text");
        Text text = textgui.GetComponent<Text>();
        text.text = buttontext;
        text.fontSize = fontsize;

    }
    */
    private void Letter_Button_Changes(string Letter, Color buttoncolor, Color textcolor) //diðer  butonlar sürekli renk deðiþtirmesi için oluþturdupum fonk
    {
        GameObject Letter_Button = GameObject.Find(Letter + "_Button");
        var colors = Letter_Button.GetComponent<Button>().colors;
        colors.normalColor = buttoncolor;
        Letter_Button.GetComponent<Button>().colors = colors;

        GameObject textgui = GameObject.Find(Letter + "_Text");
        Text text = textgui.GetComponent<Text>();
        text.color = textcolor;


    }

    private void All_Letters_Buttons_OnClick(string letter)
    {
        if (letter != ".")

        {
            if (current_column < 5)
            {
                GameObject txt = GameObject.Find("Text_R" + current_row + "_C" + current_column);

                txt.GetComponent<Text>().text = letter;
                Typed_Word += letter;
                current_column++;

                /*Submit_Button_Changes(Color.gray, "Submit", 80);
                if (current_column == 5)
                {
                    if ( Word_List.Contains(Typed_Word))
                    {
                        Submit_Button_Changes(Choosen_Color("Correct Cyan"), "Submit", 80);

                    }
                    else
                    {
                        Submit_Button_Changes(Color.red, "Yanlis Kelime", 80);

                    }
                }
                */
            }
            else
            {



            }
        }

    }

    private void Check_4_Correct_Letters()
    {

        for (int i = 0; i < Typed_Word.Length; i++)
        {
            char harf = Typed_Word[i];
            GameObject panel = GameObject.Find("panel_R" + current_row + "_C" + i);
            Image img = panel.GetComponent<Image>();
            if (Selected_Word.Contains(harf.ToString()))
            {
                if (Selected_Word[i] == harf)
                {
                    img.color = Choosen_Color("Correct Green");
                    Letter_Button_Changes(harf.ToString(), Choosen_Color("Correct Green"), Color.white);
                }
                else
                {
                    img.color = Choosen_Color("Gold");
                    Letter_Button_Changes(harf.ToString(), Choosen_Color("Gold"), Color.white);
                }
            }
            else
            {
                img.color = Color.gray;
                Letter_Button_Changes(harf.ToString(), Color.gray, Color.white);
            }
        }
    }

    private void Next()
    {
        score += guess_left;
        Score_Result_Text.GetComponent<Text>().text = score.ToString();

        Destroy(GameObject.Find("All Rows"));
        Destroy(GameObject.Find("All Buttons"));

        Create_All_Rows();
        Create_All_Buttons();

        Selected_Word = Word_List[UnityEngine.Random.Range(0, Word_List.Count)];

        guess_left = 6;

    }

    private void Game_Over_Panel()
    {
        GameOver_Panel.SetActive(true); 
        Last_Score_Result_Text.text = score.ToString();
    }


    private void Check_4_Correct_Places() //Kutucuklarý renklendirme
    {
        Debug.Log("Typed_Word length: " + Typed_Word.Length);
        int correct_count = 0;
        for (int i = 0; i < Typed_Word.Length; i++)
        {
            if (Typed_Word[i] == Selected_Word[i])
            {
                GameObject panel = GameObject.Find("panel_R" + current_row + "_C" + i);
                Image img = panel.GetComponent<Image>();
                img.color = Choosen_Color("Correct Green");
                Letter_Button_Changes(Typed_Word[i].ToString(), Choosen_Color("Correct Green"), Color.white);
                correct_count++;
            }
            else if (Selected_Word.Contains(Typed_Word[i].ToString()))
            {
                GameObject panel = GameObject.Find("panel_R" + current_row + "_C" + i);
                Image img = panel.GetComponent<Image>();
                img.color = Choosen_Color("Gold");
                Letter_Button_Changes(Typed_Word[i].ToString(), Choosen_Color("Gold"), Color.white);
            }
            else
            {
                GameObject panel = GameObject.Find("panel_R" + current_row + "_C" + i);
                Image img = panel.GetComponent<Image>();
                img.color = Color.gray;
                Letter_Button_Changes(Typed_Word[i].ToString(), Color.gray, Color.white);
            }
        }

        if (correct_count == 5)
        {
            Next();
            current_row = 0;
        }
        else
        {
            current_row++;
            guess_left--;

            if(current_row == 6 )
            {
                Game_Over_Panel();
            }
        }
        current_column = 0;
        Typed_Word = "";
    }

    


    private void Submit_Button_OnClick()
    {
        if (current_row < 6)
        {

            
            if (current_column < 5)
                return;
            
            Check_4_Correct_Places();

            
            if (current_row >= 6)
            {
                
            }
            else
            {
                
                current_column = 0;
                Typed_Word = "";
            }
        }
    }
    private void Delete_Button_OnClick()
    {
        if (current_column > 0)
        {
            GameObject txt = GameObject.Find("Text_R" + current_row + "_C" + (current_column - 1));
            txt.GetComponent<Text>().text = "";
            current_column--;
        }
    }

    private void Set_All_Groups(char[] group_of_letters, GameObject Group)
    {
        for (int i = 0; i < group_of_letters.Length; i++)
        {
            GameObject newButton = new GameObject(group_of_letters[i].ToString() + "_Button");
            newButton.transform.SetParent(Group.transform);
            RectTransform rec_4_newButton = newButton.AddComponent<RectTransform>();
            if (group_of_letters[i] != '.')
            { RectTransform_Values(rec_4_newButton, 0, 1, 0, 1, 0, 0, 90, 150); }
            else
            {
                RectTransform_Values(rec_4_newButton, 0, 1, 0, 1, 0, 0, 270, 150);
            }
            CanvasRenderer Cr_4_newButton = newButton.AddComponent<CanvasRenderer>();
            Cr_4_newButton.cullTransparentMesh = true;
            Image img = newButton.AddComponent<Image>();
            img.sprite = UISprite;

            Button button = newButton.AddComponent<Button>();
            string letter = group_of_letters[i].ToString();
            button.onClick.AddListener(() => All_Letters_Buttons_OnClick(letter));

            GameObject textGui = new GameObject(group_of_letters[i].ToString() + "_Text");
            textGui.transform.SetParent(newButton.transform);
            RectTransform rec_4_textGui = textGui.AddComponent<RectTransform>();
            RectTransform_Values(rec_4_textGui, 1, 1, 0, 0, 0, 0, 0, 0);
            CanvasRenderer Cr_4_textGui = textGui.AddComponent<CanvasRenderer>();
            Cr_4_textGui.cullTransparentMesh = true;

            Text text = textGui.AddComponent<Text>();
            if (group_of_letters[i] != '.')
            {
                text.text = group_of_letters[i].ToString();
            }
            else
            {
                text.text = "";

            }
            text.font = Font.CreateDynamicFontFromOSFont("Arial", 16);
            text.fontStyle = FontStyle.Bold;
            text.fontSize = 80;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.black;

        }

    }

    private void Group_Infos(GameObject Group, int PosX, int PosY, char[] group_of_letters)
    {
        RectTransform rec_4_Group = Group.AddComponent<RectTransform>();
        RectTransform_Values(rec_4_Group, 0.5f, 0.5f, 0.5f, 0.5f, PosX, PosY, 100, 100);

        HorizontalLayoutGroup hlc_4_Group = Group.AddComponent<HorizontalLayoutGroup>();
        HorizontalLayoutGroup_Values(hlc_4_Group, 0, 0, 0, 0, 12, TextAnchor.UpperLeft);
        Set_All_Groups(group_of_letters, Group);
    }

    private void Create_All_Buttons()
    {
        GameObject AllButtons = new GameObject("All Buttons");
        AllButtons.transform.SetParent(BackGround_Panel.transform);
        RectTransform rec_4_AllButtons = AllButtons.AddComponent<RectTransform>();
        RectTransform_Values(rec_4_AllButtons, 0.5f, 0.5f, 0.5f, 0.5f, 0, 0, 100, 100);

        GameObject First_Group = new GameObject("First Group");
        First_Group.transform.SetParent(AllButtons.transform);
        Group_Infos(First_Group, -550, -530, first_group_of_letters);

        GameObject Second_Group = new GameObject("Second Group");
        Second_Group.transform.SetParent(AllButtons.transform);
        Group_Infos(Second_Group, -500, -720, second_group_of_letters);

        GameObject Third_Group = new GameObject("Third Group");
        Third_Group.transform.SetParent(AllButtons.transform);
        Group_Infos(Third_Group, -555, -910, third_group_of_letters);

        // Submit button
        GameObject submitButton = new GameObject("Submit Button");
        submitButton.transform.SetParent(AllButtons.transform);
        RectTransform rec_4_submitButton = submitButton.AddComponent<RectTransform>();
        RectTransform_Values(rec_4_submitButton, 0.5f, 0, 0.5f, 0, 0, -1100, 300, 100); // Adjust position and size as needed

        CanvasRenderer Cr_4_submitButton = submitButton.AddComponent<CanvasRenderer>();
        Cr_4_submitButton.cullTransparentMesh = true;

        Image submitImg = submitButton.AddComponent<Image>();
        submitImg.sprite = UISprite;
        Button submitBtn = submitButton.AddComponent<Button>();

        submitBtn.onClick.AddListener(() => Submit_Button_OnClick());
        GameObject submitText = new GameObject("Submit_Button_Text");
        submitText.transform.SetParent(submitButton.transform);
        RectTransform rec_4_submitText = submitText.AddComponent<RectTransform>();
        RectTransform_Values(rec_4_submitText, 1, 1, 0, 0, 0, 0, 0, 0);
        CanvasRenderer Cr_4_submitText = submitText.AddComponent<CanvasRenderer>();
        Cr_4_submitText.cullTransparentMesh = true;

        Text submitButtonText = submitText.AddComponent<Text>();
        submitButtonText.text = "Sonraki";
        submitButtonText.font = Font.CreateDynamicFontFromOSFont("Arial", 16);
        submitButtonText.fontStyle = FontStyle.Bold;
        submitButtonText.fontSize = 45;
        submitButtonText.alignment = TextAnchor.MiddleCenter;
        submitButtonText.color = Color.black;


        GameObject deleteButton = new GameObject("Delete Button");
        deleteButton.transform.SetParent(AllButtons.transform);
        RectTransform rec_4_deleteButton = deleteButton.AddComponent<RectTransform>();
        RectTransform_Values(rec_4_deleteButton, 0.5f, 0, 0.5f, 0, 462, -886, 300, 100); // Adjust position and size as needed

        CanvasRenderer Cr_4_deleteButton = deleteButton.AddComponent<CanvasRenderer>();
        Cr_4_deleteButton.cullTransparentMesh = true;

        Image deleteImg = deleteButton.AddComponent<Image>();
        deleteImg.sprite = UISprite;
        Button deleteBtn = deleteButton.AddComponent<Button>();

        deleteBtn.onClick.AddListener(() => Delete_Button_OnClick());
        GameObject deleteText = new GameObject("Delete Text");
        deleteText.transform.SetParent(deleteButton.transform);
        RectTransform rec_4_deleteText = deleteText.AddComponent<RectTransform>();
        RectTransform_Values(rec_4_deleteText, 1, 1, 0, 0, 0, 0, 0, 0);
        CanvasRenderer Cr_4_deleteText = deleteText.AddComponent<CanvasRenderer>();
        Cr_4_deleteText.cullTransparentMesh = true;

        Text deleteButtonText = deleteText.AddComponent<Text>();
        deleteButtonText.text = "<"; 
        deleteButtonText.font = Font.CreateDynamicFontFromOSFont("Arial", 16);
        deleteButtonText.fontStyle = FontStyle.Bold;
        deleteButtonText.fontSize = 45;
        deleteButtonText.alignment = TextAnchor.MiddleCenter;
        deleteButtonText.color = Color.black;
    }

    public void NewGame_Button_Onclick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }


 
    void Start()
    {
        current_row = 0;
        current_column = 0;
        Create_All_Rows();
        Create_All_Buttons();

        Typed_Word = "";
        textFile = (TextAsset)Resources.Load(("kelimeler"), typeof(TextAsset));
        string[] WordArray = textFile.text.Split("\r\n");
        Word_List = new List<string>();
        Word_List.AddRange(WordArray);

        Selected_Word = Word_List[UnityEngine.Random.Range(0, Word_List.Count)];

        score = 0;
        guess_left = 6;
    }

    
    void Update()
    {

    }
}
