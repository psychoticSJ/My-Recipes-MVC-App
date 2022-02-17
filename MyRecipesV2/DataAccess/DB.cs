using MyRecipesV2.Models.ViewModels;
using MyRecipesV2.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MyRecipesV2.DataAccess
{
    public class DB
    {
        public IConfiguration config { get; set; }
        public DB(IConfiguration configuration)
        {
            config = configuration;
        }
        public bool SaveRecipe(RecipesViewModel recipesViewModel)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(config.GetConnectionString("dbConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("spSaveRecipe", sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("Id", recipesViewModel.Recipe.Id);
                    cmd.Parameters.AddWithValue("Name", recipesViewModel.Recipe.Name);
                    cmd.Parameters.AddWithValue("Description", recipesViewModel.Recipe.Description);
                    cmd.Parameters.AddWithValue("Ingredients", recipesViewModel.Recipe.Ingredients);
                    cmd.Parameters.AddWithValue("CategoryId", recipesViewModel.Recipe.Category);
                    if (recipesViewModel.Recipe.SubCategory == null)
                        cmd.Parameters.AddWithValue("SubCategoryId", 0);
                    else
                        cmd.Parameters.AddWithValue("SubCategoryId", recipesViewModel.Recipe.SubCategory);
                    cmd.Parameters.AddWithValue("NumberOfServings", recipesViewModel.Recipe.NumberOfServings);
                    cmd.Parameters.AddWithValue("PrepTime", recipesViewModel.Recipe.PrepTime);
                    cmd.Parameters.AddWithValue("CookTime", recipesViewModel.Recipe.CookTime);
                    cmd.Parameters.AddWithValue("TotalTime", recipesViewModel.Recipe.PrepTime + recipesViewModel.Recipe.CookTime);
                    cmd.Parameters.AddWithValue("Step1", recipesViewModel.Recipe.Step1);
                    cmd.Parameters.AddWithValue("Step2", recipesViewModel.Recipe.Step2);
                    if (recipesViewModel.Recipe.Step3 == null)
                        cmd.Parameters.AddWithValue("Step3", "-");
                    else
                        cmd.Parameters.AddWithValue("Step3", recipesViewModel.Recipe.Step3);
                    if (recipesViewModel.Recipe.Step4 == null)
                        cmd.Parameters.AddWithValue("Step4", "-");
                    else
                        cmd.Parameters.AddWithValue("Step4", recipesViewModel.Recipe.Step4);
                    if (recipesViewModel.Recipe.Step5 == null)
                        cmd.Parameters.AddWithValue("Step5", "-");
                    else
                        cmd.Parameters.AddWithValue("Step5", recipesViewModel.Recipe.Step5);
                    cmd.Parameters.AddWithValue("NutritionFacts", recipesViewModel.Recipe.NutritionFacts);
                    cmd.Parameters.AddWithValue("Image1Url", recipesViewModel.Recipe.Image1URL);
                    cmd.Parameters.AddWithValue("Image2Url", recipesViewModel.Recipe.Image2URL);
                    cmd.Parameters.AddWithValue("Image3Url", recipesViewModel.Recipe.Image3URL);
                    cmd.Parameters.AddWithValue("Image4Url", recipesViewModel.Recipe.Image4URL);
                    cmd.Parameters.AddWithValue("Image5Url", recipesViewModel.Recipe.Image5URL);
                    if (recipesViewModel.Recipe.VideoURL == null)
                        cmd.Parameters.AddWithValue("VideoURL", "-");
                    else
                        cmd.Parameters.AddWithValue("VideoURL", recipesViewModel.Recipe.VideoURL);
                    cmd.Parameters.AddWithValue("Tags", recipesViewModel.Recipe.Tags);
                    cmd.Parameters.AddWithValue("SubmittedBy", "Shamzy");
                    cmd.Parameters.AddWithValue("SubmittedDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                var error = ex.Message.ToString();
                return false;
            }
        }

        internal IEnumerable<SubCategories> GetAllSubCategories()
        {
            DataTable dtSubCategories = new DataTable();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(config.GetConnectionString("dbConnection")))
                {
                    sqlConnection.Open();
                    SqlDataAdapter daSubCategories = new SqlDataAdapter("select * from tblCategories where IsSubCategory = 1", sqlConnection);
                    daSubCategories.SelectCommand.CommandType = CommandType.Text;
                    daSubCategories.Fill(dtSubCategories);

                    var subCategoryNames = new List<SubCategories>();
                    foreach (DataRow row in dtSubCategories.Rows)
                    {
                        subCategoryNames.Add(new SubCategories()
                        {
                            SubCategoryDescription = row["CategoryDescription"].ToString(),
                            Id = (int)row["CategoryID"]
                        });
                    }
                    return subCategoryNames;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<Categories> GetAllCategories()
        {
            DataTable dtCategories = new DataTable();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(config.GetConnectionString("dbConnection")))
                {
                    sqlConnection.Open();
                    SqlDataAdapter daCategories = new SqlDataAdapter("select * from tblCategories where IsMainCategory = 1", sqlConnection);
                    daCategories.SelectCommand.CommandType = CommandType.Text;
                    daCategories.Fill(dtCategories);
                    
                    var categoryNames = new List<Categories>();
                    foreach (DataRow row in dtCategories.Rows)
                    {
                        categoryNames.Add(new Categories()
                        {
                            CategoryDescription = row["CategoryDescription"].ToString(),
                            Id = (int)row["CategoryID"]
                        });
                    }
                    return categoryNames;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetAllRecipes()
        {
            DataTable dtRecipes = new();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(config.GetConnectionString("dbConnection")))
                {
                    sqlConnection.Open();
                    SqlDataAdapter daRecipes = new SqlDataAdapter("select * from vwRecipes", sqlConnection);
                    daRecipes.SelectCommand.CommandType = CommandType.Text;
                    daRecipes.Fill(dtRecipes);
                    return dtRecipes;
                }
            }
            catch (Exception)
            {
                return dtRecipes;
            }
        }
        

        public Recipes GetRecipe(int? id)
        {
            DataTable dtRecipes = new DataTable();
            Recipes recipe = new Recipes();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(config.GetConnectionString("dbConnection")))
                {
                    sqlConnection.Open();
                    SqlDataAdapter daRecipes = new SqlDataAdapter($"select * from vwRecipes where Id = {id}", sqlConnection);
                    daRecipes.SelectCommand.CommandType = CommandType.Text;
                    daRecipes.Fill(dtRecipes);
                    recipe.Id = Convert.ToInt32(dtRecipes.Rows[0]["id"]);
                    recipe.Name = dtRecipes.Rows[0]["Name"] as string ?? "";
                    recipe.Description = dtRecipes.Rows[0]["Description"] as string ?? "";
                    recipe.Ingredients = dtRecipes.Rows[0]["Ingredients"] as string ?? "";
                    recipe.Category = dtRecipes.Rows[0]["id"] as int? ?? default;
                    recipe.SubCategory = dtRecipes.Rows[0]["id"] as int? ?? default;
                    recipe.NumberOfServings = dtRecipes.Rows[0]["id"] as int? ?? default;
                    recipe.PrepTime = dtRecipes.Rows[0]["PrepTime"] as int? ?? default;
                    recipe.CookTime = dtRecipes.Rows[0]["CookTime"] as int? ?? default;
                    recipe.TotalTime = dtRecipes.Rows[0]["TotalTime"] as int? ?? default;
                    recipe.Step1 = dtRecipes.Rows[0]["Step1"] as string ?? "";
                    recipe.Step2 = dtRecipes.Rows[0]["Step2"] as string ?? "";
                    recipe.Step3 = dtRecipes.Rows[0]["Step3"] as string ?? "";
                    recipe.Step4 = dtRecipes.Rows[0]["Step4"] as string ?? "";
                    recipe.Step5 = dtRecipes.Rows[0]["Step5"] as string ?? "";
                    recipe.NutritionFacts = dtRecipes.Rows[0]["NutritionFacts"] as string ?? "";
                    recipe.Image1URL = dtRecipes.Rows[0]["Image1URL"] as string ?? "";
                    recipe.Image2URL = dtRecipes.Rows[0]["Image2URL"] as string ?? "";
                    recipe.Image3URL = dtRecipes.Rows[0]["Image3URL"] as string ?? "";
                    recipe.Image4URL = dtRecipes.Rows[0]["Image4URL"] as string ?? "";
                    recipe.Image5URL = dtRecipes.Rows[0]["Image5URL"] as string ?? "";
                    recipe.VideoURL = dtRecipes.Rows[0]["VideoURL"] as string ?? "";
                    recipe.Tags = dtRecipes.Rows[0]["Tags"] as string ?? "";
                    return recipe;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
