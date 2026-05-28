using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    public string nombreEscenaMapa = "ACR Biblio";

    public void IniciarJuego()
    {
        SceneManager.LoadScene(nombreEscenaMapa);
    }
}