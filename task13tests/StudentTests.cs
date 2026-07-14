using System;
using System.Collections.Generic;
using System.Text;
using task13;

namespace task13tests
{
    public class StudentTests
    {
        [Fact]
        public void Deserialize_EmptyFirstName_ShouldThrowArgumentException()
        {
            string studentJsonData = "{\"FirstName\": \"\", \"BirthDate\": \"2004-05-10\"}";

            var thrownException = Assert.Throws<ArgumentException>(() => StudentService.DeserializeStudent(studentJsonData));
            Assert.Contains("Ошибка", thrownException.Message);
        }

        [Fact]
        public void Deserialize_GradeAboveMaximum_ShouldThrowArgumentOutOfRangeException()
        {
            string studentJsonData = @"{
        ""FirstName"": ""Ольга"",
        ""BirthDate"": ""2004-08-20"",
        ""Grades"": [
            { ""Name"": ""Биология"", ""Grade"": 150 }
        ]
    }";

            var thrownException = Assert.Throws<ArgumentOutOfRangeException>(() => StudentService.DeserializeStudent(studentJsonData));
            Assert.Contains("Баллы вне диапазона", thrownException.Message);
        }
        [Fact]
        public void Deserialize_WhiteSpaceFirstName_ShouldThrowArgumentException()
        {
            string studentJsonData = "{\"FirstName\": \"   \", \"BirthDate\": \"2004-05-10\"}";

            var thrownException = Assert.Throws<ArgumentException>(() => StudentService.DeserializeStudent(studentJsonData));
            Assert.Contains("Пустое значение", thrownException.Message);
        }
        [Fact]
        public void Deserialize_EmptySubjectName_ShouldThrowArgumentException()
        {
            string studentJsonData = @"{
        ""FirstName"": ""Сергей"",
        ""BirthDate"": ""2003-06-15"",
        ""Grades"": [
            { ""Name"": """", ""Grade"": 85 }
        ]
    }";

            var thrownException = Assert.Throws<ArgumentException>(() => StudentService.DeserializeStudent(studentJsonData));
            Assert.Contains("Пустое значение", thrownException.Message);
        }
    }
}
