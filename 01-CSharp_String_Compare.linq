<Query Kind="Program">
  <Connection>
    <ID>a4c486a3-f76d-44dd-99be-6743d630bf9a</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Cookies</Database>
    <Server>.\SQL2016</Server>
  </Connection>
  <Namespace>System.Threading.Tasks</Namespace>
  <RuntimeVersion>6.0</RuntimeVersion>
</Query>

void Main()
{
	TestByDb1();	// 查詢條件: 常數
	TestByDb2();	// 查詢條件: 變數
	TestByDb2();	// 查詢條件: 多個變數
	TestByList1();	// 查詢條件: 常數
	TestByList2();	// 查詢條件: 變數
	TestByList3();	// 查詢條件: 多個變數
}

// You can define other methods, fields, classes and namespaces here
public class CityModel 
{
	public string CityId {get; set;}
	public string CityName {get; set;}
}

public class EmployeeModel
{
	public string EmployeeId {get; set;}
	public string EmployeeName {get; set;}
	public string CityId {get; set;}
}

public class VWEmpCityModel
{
	public string EmployeeId { get; set; }
	public string EmployeeName { get; set; }
	public string CityId { get; set; }
	public string CityName { get; set; }
}

/// <summary>
/// 由資料庫讀取
/// </summary>
/// <remarks>
/// 目的: 驗證會在 SQL 指令, 產生 WHERE 條件 (常數)
/// <remarks>
private void TestByDb1()
{
	// 將 LINQPad 的 DataContext 實例顯式賦值給變數
	var db = this; // `this` 是 UserQuery，但它包含 DataContext 的功能

	// CASE 1: 不能這樣寫, 會出現 CS0019 Operator '>=' cannot be applied to operands of type 'string' and 'string'
	// var query = dataContext.VWEmpCities.Where(x => x.CityId >= "5");

	// CASE 2: 字串的比對, 要改用 CompareTo 的方式處理; 會轉成如下的 SQL.
	// -- Region Parameters
	// DECLARE @p0 VarChar(1000) = '5'
	// -- EndRegion
	// SELECT[t0].[EmployeeId], [t0].[EmployeeName], [t0].[CityId], [t0].[CityName]
	// FROM[VWEmpCities] AS[t0]
	// WHERE [t0].[CityId] >= @p0
	var query = db.VWEmpCities.Where(x => x.CityId.CompareTo("5") >= 0);
	var result = query.ToList();
	result.Dump("TestByDb1");
}

/// <summary>
/// 由資料庫讀取
/// </summary>
/// <remarks>
/// 目的: 驗證會在 SQL 指令, 產生 WHERE 條件 (變數)
/// <remarks>
private void TestByDb2()
{
	var db = this;

	var userInput = "5";
	var query = db.VWEmpCities.Where(x => x.CityId.CompareTo(userInput) >= 0);
	var result = query.ToList();
	result.Dump("TestByDb2");
}

/// <summary>
/// 由資料庫讀取
/// </summary>
/// <remarks>
/// 目的: 驗證會在 SQL 指令, 產生 WHERE 條件 (多個變數)
/// <remarks>
private void TestByDb3()
{
	var db = this;

	var userInput = "5";
	var nameStart = "j";
	var query = db.VWEmpCities.Where(x => x.CityId.CompareTo(userInput) >= 0 && x.EmployeeName.StartsWith(nameStart));
	var result = query.ToList();
	result.Dump("TestByDb3");
}

/// <summary>
/// 由資料庫取出全部資料, 再作過濾
/// </summary>
/// <remarks> 
/// 註: 條件採用常數值
/// </remarks>
private void TestByList1()
{
	var db = this;
	var allQuery = db.VWEmpCities;
	var allData = allQuery.Select(emp => new VWEmpCityModel
	{
		EmployeeId = emp.EmployeeId,
		EmployeeName = emp.EmployeeName,
		CityId = emp.CityId,
		CityName = emp.CityName
	}).ToList();
	
	var query = allData.Where(x => x.CityId.CompareTo("5") >= 0);
	var result = query.ToList();
	result.Dump("TestByList1");
}

/// <summary>
/// 由資料庫取出全部資料, 再作過濾
/// </summary>
/// <remarks> 
/// 註: 條件採用變數值
/// </remarks>
private void TestByList2()
{
	var db = this;
	var allQuery = db.VWEmpCities;
	var allData = allQuery.Select(emp => new VWEmpCityModel
	{
		EmployeeId = emp.EmployeeId,
		EmployeeName = emp.EmployeeName,
		CityId = emp.CityId,
		CityName = emp.CityName
	}).ToList();

	var userInput = "5";
	var query = allData.Where(x => x.CityId.CompareTo(userInput) >= 0);
	var result = query.ToList();
	result.Dump("TestByList2");
}


/// <summary>
/// 由資料庫取出全部資料, 再作過濾
/// </summary>
/// <remarks> 
/// 註: 條件採用變數值 (多個條件)
/// </remarks>
private void TestByList3()
{
	var db = this;
	var allQuery = db.VWEmpCities;
	var allData = allQuery.Select(emp => new VWEmpCityModel
	{
		EmployeeId = emp.EmployeeId,
		EmployeeName = emp.EmployeeName,
		CityId = emp.CityId,
		CityName = emp.CityName
	}).ToList();

	var userInput = "5";
	var nameStart = "j";
	var query = allData.Where(x => x.CityId.CompareTo(userInput) >= 0 && x.EmployeeName.StartsWith(nameStart) );
	var result = query.ToList();
	result.Dump("TestByList3");
}