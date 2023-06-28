using UnityEngine;

public class ChangeSkinMaps : MonoBehaviour
{

    //���� ������ �� ������ �����, �� �� ������� ������� ChangeSkinPlayer ������� ���� -1
    [ContextMenu("Down")]
    public void Down()
    {
        // ������� ������ � ����������� ChangeSkinPlayer
        GameObject player = GameObject.Find("ChangeSkinPlayer");
        ChangeSkinPlayer script = player.GetComponent<ChangeSkinPlayer>();

        //������� ����� �����
        script.ChangeSkinDown();
    }

    [ContextMenu("Next")]
    //���� ������ �� ������ �����, �� �� ������� ������� ChangeSkinPlayer ������� ���� +1
    public void Next()
    {
        // ������� ������ � ����������� ChangeSkinPlayer
        GameObject player = GameObject.Find("ChangeSkinPlayer");
        ChangeSkinPlayer script = player.GetComponent<ChangeSkinPlayer>();

        //������� ����� �����
        script.ChangeSkinNext();
    }






}
