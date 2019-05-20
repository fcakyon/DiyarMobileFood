using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Api : MonoBehaviour {

    public static readonly string DecorModels = "https://diyar-server.herokuapp.com/v0/models/?category=decor";
    public static readonly string FoodModels = "https://diyar-server.herokuapp.com/v0/models/?category=food";
    public static readonly string AllModels = "https://diyar-server.herokuapp.com/v0/models/"; // Dynamic links uses this so '/' at the end is mandatory
    public static readonly string DecorModelsLocal = "http://localhost:3000/v0/models/?category=decor";
    public static readonly string FoodModelsLocal = "http://localhost:3000/v0/models/?category=food";
    public static readonly string AllModelsLocal = "http://localhost:3000/v0/models/"; 
}
