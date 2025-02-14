using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SQLConnectionManager : Singleton<SQLConnectionManager>
{
    private void Start()
    {
        GetData();
    }

    public void GetData()
    {
        // �������� ��������������� ������� �������
        DataTable table = MyDataBase.GetTable("SELECT * FROM Customers ORDER BY bonus DESC;");
        // �������� id ������� ������
        int idBestCustomer = int.Parse(table.Rows[0][0].ToString());
        // �������� ��� ������� ������
        string nickname = MyDataBase.ExecuteQueryWithAnswer($"SELECT name FROM Customers WHERE id = {idBestCustomer};");
        Debug.Log($"Best customer {nickname} gained {table.Rows[0][3].ToString()} bonuses.");

        MainMenuUIManager.Instance.debugTextController.SetDebugText($"Best customer {nickname} gained {table.Rows[0][3].ToString()} bonuses.");
    }
}
