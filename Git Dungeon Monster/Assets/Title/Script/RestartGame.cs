using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
 public void Restart()
    {
        // ���݂�Scene���擾
        Scene loadScene = SceneManager.GetActiveScene();
        // ���݂̃V�[�����ēǂݍ��݂���
        SceneManager.LoadScene("Stage1");
    }
}
