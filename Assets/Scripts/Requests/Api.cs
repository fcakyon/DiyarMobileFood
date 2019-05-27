using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Api : MonoBehaviour {

    #region REMOTE SERVER
    public static readonly string DecorModels = "https://diyar-server.herokuapp.com/v0/models/?category=decor";
    public static readonly string FoodModels = "https://diyar-server.herokuapp.com/v0/models/?category=food";
    public static readonly string AllModels = "https://diyar-server.herokuapp.com/v0/models/"; // Dynamic links uses this so '/' at the end is mandatory
    public static readonly string DecorSubCategories = "https://diyar-server.herokuapp.com/v0/models/categories?category=decor"; 
    public static string DecorModelsBySubCategory(string subCategory)
    {
        return "https://diyar-server.herokuapp.com/v0/models/?category=decor&subCategory=" + subCategory;
    }
    #endregion

    #region LOCAL APIs 
    public static readonly string DecorModelsLocal = "http://localhost:3000/v0/models/?category=decor";
    public static readonly string FoodModelsLocal = "http://localhost:3000/v0/models/?category=food";
    public static readonly string AllModelsLocal = "http://localhost:3000/v0/models/";
    public static readonly string DecorSubCategoriesLocal = "http://localhost:3000/v0/models/categories?category=decor";
    public static string DecorModelsBySubCategoryLocal(string subCategory)
    {
        return "http://localhost:3000/v0/models/?category=decor&subCategory=" + subCategory;
    }
    #endregion
}
