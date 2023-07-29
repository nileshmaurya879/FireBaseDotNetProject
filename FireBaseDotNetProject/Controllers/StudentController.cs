using FireBaseDotNetProject.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FireBaseDotNetProject.Controllers
{
    public class StudentController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            BasePath = "https://studentdotnetfirebaseapp-default-rtdb.firebaseio.com/",
            AuthSecret = "5hkWd4yoT2zy7Fyh1qsQA2XHK9BLhp6tzAYli7ox"
        };
        IFirebaseClient client;

        public IActionResult Index()
        {

            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Student");
            dynamic student = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Student>();
            if(student != null)
            {
                foreach (var item in student)
                {
                    list.Add(JsonConvert.DeserializeObject<Student>(((JProperty)item).Value.ToString()));
                }
            }
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Student student)
        {
            client = new FireSharp.FirebaseClient(config);

            var data = student;
            PushResponse response = client.Push("Student/", data);
            data.Id = response.Result.name;

            SetResponse setResponse = client.Set("Student/" + data.Id, data);

            if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ModelState.AddModelError(string.Empty, "Added Succesfully");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Something went wrong!!");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("Student/" + id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Student/" + id);
            Student data = JsonConvert.DeserializeObject<Student>(response.Body);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(Student student)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("Student/" + student.Id, student);
            return RedirectToAction("Index");
        }
    }
}
