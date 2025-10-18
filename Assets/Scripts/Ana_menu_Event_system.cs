using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Ana_menu_Event_system : MonoBehaviour
{
       //31315431
       //erkenkalktım
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void oyunu_kapat()
        {
        // Unity Editöründe çalışırken durdurmak için
        Application.Quit();
        }   

    public void start()
    {
        SceneManager.LoadScene(1);
    }

}
