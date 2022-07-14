using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
 public void Restart()
    {
        // Œ»İ‚ÌScene‚ğæ“¾
        Scene loadScene = SceneManager.GetActiveScene();
        // Œ»İ‚ÌƒV[ƒ“‚ğÄ“Ç‚İ‚İ‚·‚é
        SceneManager.LoadScene("Stage1");
    }
}
