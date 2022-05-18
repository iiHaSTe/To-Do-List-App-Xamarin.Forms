using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Bootstrap;

namespace com.haste.todoList
{
  public partial class MainPage : ContentPage
  {
    public List<String> TaskListNames = new List<String>() { };
    public MainPage()
    {
      InitializeComponent();
      Add.Clicked += AddTask;
      TaskName.Completed += (sender, e)=>{
        TaskName.Focus();
        AddTask(sender, e);
        TaskName.Focus();
      };
      ClearAll.Clicked += async delegate
      {
        if (TaskListNames.Count == 0)
        {
          Toast.MakeText("list is empty".ToUpper(), Toast.ToastLength.Long).Show();
          return;
        }
        bool sure = await DisplayAlert("warning".ToUpper(), "Sure For Removing All The Tasks", "Yes", "No");
        if (sure)
        {
          TaskListNames.Clear();
          ReloadTasks();
          
          Toast.MakeText("cleared".ToUpper(), Toast.ToastLength.Long).Show();
          return;
        }
      };
    }
    
    public void RemoveTask(Object sender, EventArgs e)
    {
      Button btn = (Button)sender;
      TaskListNames.Remove(TaskListNames[int.Parse(btn.ClassId)]);
      
      ReloadTasks();
    }
    
    public void AddTask(object sender, EventArgs e)
    {
      TaskName.Focus();
      if (String.IsNullOrEmpty(TaskName.Text))
      {
        DisplayAlert("WARNING", "pliiz set the task name", "ok");
        return;
      }
      
      if (TaskListNames.Contains(TaskName.Text))
      {
        Toast.MakeText("can't add the task".ToUpper(), Toast.ToastLength.Long).Show();
        TaskName.Text = "";
        return;
      }
      TaskListNames.Add(TaskName.Text);
      TaskName.Text = "";
      
      TaskName.Focus();
      ReloadTasks();
      TaskName.Focus();
    }
    
    public void ReloadTasks()
    {
      TaskList.Children.Clear();
      
      int j = 0;
      
      foreach (KeyValuePair<int, String> i in TaskListNames.Distinct().ToDictionary(x => j++, y => y))
      {
        var myLabel = new Label()
        {
          Text = i.Value,
          BackgroundColor = Color.FromHex("#222"),
          TextColor = Color.White,
          Padding = 13,
          HorizontalTextAlignment = TextAlignment.Start,
        };
        
        var indexLabel = new Label()
        {
          Text = i.Key.ToString(),
          BackgroundColor = Color.FromHex("#222"),
          TextColor = Color.White,
          Padding = 13,
          HorizontalTextAlignment = TextAlignment.Center,
        };
        
        var rmButton = new Button()
        {
          Text = "x",
          TextColor = Color.White,
          BackgroundColor = Color.Red,
          FontAttributes = FontAttributes.Bold,
          ClassId = i.Key.ToString(),
          BorderRadius=0
        };
        
        var myGrid = new Grid()
        {
          RowSpacing = 1,
          ColumnSpacing = 1,
          ColumnDefinitions ={
              new ColumnDefinition(){
                Width=new GridLength(1, GridUnitType.Star)
              },
              new ColumnDefinition(){
                Width=new GridLength(4, GridUnitType.Star)
              },
              new ColumnDefinition(){
                Width=new GridLength(1, GridUnitType.Star)
              }
            },
          Children ={
              myLabel,
              rmButton,
              indexLabel,
            },
        };
        Grid.SetColumn(myLabel, 1);
        Grid.SetColumn(rmButton, 2);
        Grid.SetColumn(indexLabel, 0);
        
        rmButton.Clicked += RemoveTask;
        
        TaskList.Children.Add(myGrid);
      }
    }
  }
}




