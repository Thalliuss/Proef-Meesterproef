using DataManagement;
using UnityEngine;

public class TreeHandler : MonoBehaviour
{
    [SerializeField]
    private int _health = 100;

    public void TakeDamage(int p_input)
    {
        _health -= p_input;
        if (_health <= 0) {
            SceneManager t_sceneManager = SceneManager.Instance;
            DataReferences t_dataReferences = t_sceneManager.DataReferences;

            Destroy(gameObject);
            TreeInfo t_treeInfoArray = t_dataReferences.FindElement<TreeInfo>("TREE_DATA");
            t_treeInfoArray.Save();
        }
    }
}
