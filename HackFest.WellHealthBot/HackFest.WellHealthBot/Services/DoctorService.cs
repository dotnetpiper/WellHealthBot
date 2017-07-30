using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackFest.WellHealthBot.Models.Doctor;

namespace HackFest.WellHealthBot.Services
{
    public class DoctorService
    {
        public static async Task<IEnumerable<Doctor>> FindDoctorsAsync(SearchDoctorQuery searchQuery)
        {
            var doctors = new List<Doctor>();

            // Filling the hotels results manually just for demo purposes
            for (var i = 1; i <= 4; i++)
            {
                var random = new Random(i);
                var docEntity = DummyDoctors()[i];
                var doc = new Doctor
                {
                    Key= docEntity.Key,
                    Page = docEntity.Page,
                    Name = docEntity.Name,
                    Location = searchQuery.Location,
                    Rating = docEntity.Rating,
                    NumberOfReviews = random.Next(0, 5000),
                    Image = docEntity.Image
                    //$"https://placeholdit.imgix.net/~text?txtsize=35&txt=Doctor+{i}&w=500&h=260"
                };

                doctors.Add(doc);
            }

            doctors = doctors.OrderByDescending(c => c.Rating).ToList();

            return doctors;
        }

        private static List<Doctor> DummyDoctors()
        {
            var lstDoctors = new List<Doctor>
            {
                new Doctor
                {
                    Name = "Dr. Nitin Jha",
                    Image = "https://images1-fabric.practo.com/dr-nitin-jha-1452774116-569792e44aa8b.jpg/thumbnail",
                    Rating = 5,
                    Key = "1",
                    Page = "https://www.practo.com/noida/doctor/dr-nitin-jha-bariatric-surgeon?results_type=doctor"
                },
                new Doctor
                {
                    Name = "Dr. Gaurav Rathore",
                    Image = "https://images1-fabric.practo.com/dr-gaurav-rathore-1469084246-57907257001a9.jpg/thumbnail",
                    Rating = 4,
                    Key = "2",
                    Page = "https://www.practo.com/noida/doctor/dr-gaurav-rathore-orthopedist?results_type=doctor"
                },
                new Doctor
                {
                    Name = "Dr. Manu Tiwari",
                    Image =
                        "https://images1-fabric.practo.com/554a117e9403d8277e0910d750195b448797616e091ad.jpg/thumbnail",
                    Rating = 1,
                    Key = "3",
                    Page = "https://www.practo.com/noida/doctor/manu-tiwari-psychiatrist?results_type=doctor"
                },
                new Doctor
                {
                    Name = "Dr. Ali Nawaz",
                    Image = "https://images1-fabric.practo.com/dr-ali-nawaz-1469440404-5795e1940b74e.jpg/thumbnail",
                    Rating = 3,
                    Key = "4",
                    Page = "https://www.practo.com/noida/doctor/dr-ali-nawaz-general-physician-1?results_type=doctor"
                },
                new Doctor
                {
                    Name = "Dr. Aradhana Singh",
                    Image =
                        "https://images1-fabric.practo.com/doctor/449066/dr-aradhana-singh-58e381e4d3abf.jpg/thumbnail",
                    Rating = 2,
                    Key = "5",
                    Page = "https://www.practo.com/noida/doctor/dr-aradhana-singh-gynecologist-obstetrician?results_type=doctor"
                }
            };
            return lstDoctors;
        }
    }
}