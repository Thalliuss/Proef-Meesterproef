using DataManagement;
using UnityEngine;

public class SaveTree : MonoBehaviour
{
    private SceneManger _sceneManager;
    private DataReferences _dataReferences;

    private const string _treeDataArrayID = "TREE_DATA";

    void Start ()
    {
        _sceneManager = SceneManger.Instance;
        _dataReferences = _sceneManager.DataReferences;

        TreeInfo t_treeInfoArray = _dataReferences.FindElement<TreeInfo>(_treeDataArrayID);
        if (t_treeInfoArray == null)
        {
            _dataReferences.AddElement<TreeInfo>(new TreeInfo(_treeDataArrayID));
            t_treeInfoArray = _dataReferences.FindElement<TreeInfo>(_treeDataArrayID);
        }
        if (t_treeInfoArray == null) return;
        if (t_treeInfoArray.Trees.Count > GenerationManager.Instance.Amount) return;

        Tree t_obj = new Tree(transform.position, transform.rotation, GenerationManager.Instance.Tree);

        t_treeInfoArray.Trees.Add(t_obj);
        t_treeInfoArray.Save();
    }
}
