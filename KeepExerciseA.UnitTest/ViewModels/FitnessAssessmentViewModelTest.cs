using KeepExerciseA.Library.ViewModels;

namespace KeepExerciseA.UnitTest.ViewModels;
 public class FitnessAssessmentViewModelTest
    {
        private FitnessAssessmentViewModel CreateViewModel()
        {
            return new FitnessAssessmentViewModel();
        }

        [Fact]
        public void GenerateReport_WithAllEmptyInputs_ShowsErrorMessage()
        {
            
            var vm = CreateViewModel();
            
            vm.GenerateReportCommand.Execute(null);
            
            Assert.Equal("输入错误：请确保所有字段都填写了有效的数字。", vm.ReportText);
        }

        [Theory]
        [InlineData("abc", "70", "15", "90", "80", "95", "30")] 
        [InlineData("175", "-70", "15", "90", "80", "95", "30")] 
        [InlineData("175", "70", "150", "90", "80", "95", "30")] 
        [InlineData("175", "70", "15", "0", "80", "95", "30")] 
        [InlineData("175", "70", "15", "90", "0", "95", "30")] 
        [InlineData("175", "70", "15", "90", "80", "0", "30")] 
        [InlineData("175", "70", "15", "90", "80", "95", "0")] 
        public void GenerateReport_WithInvalidInput_ShowsErrorMessage(
            string height, string weight, string bodyFat, string chest, string waist, string hips, string arm)
        {
            var vm = CreateViewModel();
            vm.Height = height;
            vm.Weight = weight;
            vm.BodyFatPercentage = bodyFat;
            vm.Gender = "男";
            vm.Chest = chest;
            vm.Waist = waist;
            vm.Hips = hips;
            vm.Arm = arm;
            
            vm.GenerateReportCommand.Execute(null);
            
            Assert.Equal("输入错误：请确保所有字段都填写了有效的数字。", vm.ReportText);
        }

        [Fact]
        public void GenerateReport_WithValidNormalMaleData_GeneratesCorrectReport()
        {
            var vm = CreateViewModel();
            vm.Height = "175";
            vm.Weight = "70";
            vm.BodyFatPercentage = "18";
            vm.Gender = "男";
            vm.Chest = "95";
            vm.Waist = "80";
            vm.Hips = "95";
            vm.Arm = "30";
            
            vm.GenerateReportCommand.Execute(null);
            
            var report = vm.ReportText;
            Assert.Contains("您的健康评估报告", report);
            Assert.Contains("性别: 男", report);
            Assert.Contains("身高: 175.0 cm", report);
            Assert.Contains("体重: 70.0 kg", report);
            Assert.Contains("体脂率: 18.0%", report);
            
            Assert.Contains("您的BMI指数为: 22.86", report);
            Assert.Contains("评价: 您的体重状况属于【正常】范围。", report);
            Assert.Contains("恭喜您！请继续保持健康的生活方式", report);
            
            Assert.Contains("评价: 您的体脂率属于【标准】范围。", report);
            Assert.Contains("您的体脂率在健康范围内，继续保持。", report);
            
            Assert.Contains("您的腰臀比为: 0.84", report);
            Assert.Contains("评价: 您患心血管疾病的风险为【低风险】。", report);
            Assert.Contains("您的脂肪分布较为健康，请继续保持。", report);
        }

        [Fact]
        public void GenerateReport_WithValidUnderweightFemaleData_GeneratesCorrectAdvice()
        {
            var vm = CreateViewModel();
            vm.Height = "170";
            vm.Weight = "50"; 
            vm.BodyFatPercentage = "20";
            vm.Gender = "女";
            vm.Chest = "85";
            vm.Waist = "65";
            vm.Hips = "88";
            vm.Arm = "25";
            
            vm.GenerateReportCommand.Execute(null);
            
            var report = vm.ReportText;
            Assert.Contains("您的BMI指数为: 17.30", report);
            Assert.Contains("评价: 您的体重状况属于【偏瘦】范围。", report);
            Assert.Contains("建议：适当增加营养摄入，进行适度的力量训练以增加肌肉量。", report);
            Assert.Contains("建议增加营养摄入，保证充足的优质蛋白质和健康脂肪", report);
        }

        [Fact]
        public void GenerateReport_WithValidOverweightMaleData_GeneratesCorrectAdvice()
        {
            var vm = CreateViewModel();
            vm.Height = "175";
            vm.Weight = "85"; 
            vm.BodyFatPercentage = "26"; 
            vm.Gender = "男";
            vm.Chest = "105";
            vm.Waist = "95"; 
            vm.Hips = "100";
            vm.Arm = "33";
            
            vm.GenerateReportCommand.Execute(null);
            
            var report = vm.ReportText;
            Assert.Contains("您的BMI指数为: 27.76", report);
            Assert.Contains("评价: 您的体重状况属于【超重】范围。", report);
            Assert.Contains("建议：注意控制饮食，减少高热量食物摄入，并增加有氧运动。", report);

            Assert.Contains("评价: 您的体脂率属于【过高】范围。", report);
            Assert.Contains("体脂率偏高，增加力量训练来提升肌肉量是降低体脂的有效途径", report);
            
            Assert.Contains("您的腰臀比为: 0.95", report);
            Assert.Contains("评价: 您患心血管疾病的风险为【中等风险】。", report);
            Assert.Contains("风险适中，可以通过调整饮食和增加运动来进一步降低风险。", report);
            
            Assert.Contains("建议适当控制总热量摄入，增加蔬菜和蛋白质的比例", report);
            Assert.Contains("建议将有氧运动（如慢跑、游泳、骑行）与力量训练相结合", report);
        }
        
        [Fact]
        public void GenerateReport_WithHighBodyFatMaleData_GeneratesCorrectAdvice()
        {
            var vm = CreateViewModel();
            vm.Height = "180";
            vm.Weight = "80"; 
            vm.BodyFatPercentage = "22"; 
            vm.Gender = "男";
            vm.Chest = "100";
            vm.Waist = "88";
            vm.Hips = "98";
            vm.Arm = "31";
            
            vm.GenerateReportCommand.Execute(null);
            
            var report = vm.ReportText;
            Assert.Contains("评价: 您的体重状况属于【超重】范围。", report);
            
            Assert.Contains("评价: 您的体脂率属于【偏高】范围。", report);
            Assert.Contains("体脂率偏高，增加力量训练来提升肌肉量是降低体脂的有效途径", report);
        }
        
        [Fact]
        public void GenerateReport_WithHighWHRiskMaleData_GeneratesCorrectAdvice()
        {
            var vm = CreateViewModel();
            vm.Height = "175";
            vm.Weight = "85";
            vm.BodyFatPercentage = "26";
            vm.Gender = "男";
            vm.Chest = "105";
            vm.Waist = "100"; 
            vm.Hips = "95";
            vm.Arm = "33";
            
            vm.GenerateReportCommand.Execute(null);
            
            var report = vm.ReportText;
            Assert.Contains("您的腰臀比为: 1.05", report);
            Assert.Contains("评价: 您患心血管疾病的风险为【高风险】。", report);
            Assert.Contains("中心性肥胖（苹果型身材）与健康风险关联较大，请特别关注腰围", report);
        }

        [Fact]
        public void GenerateReport_WithValidObeseFemaleData_GeneratesCorrectAdvice()
        {
            var vm = CreateViewModel();
            vm.Height = "160";
            vm.Weight = "80"; 
            vm.BodyFatPercentage = "35";
            vm.Gender = "女";
            vm.Chest = "110";
            vm.Waist = "95";
            vm.Hips = "105";
            vm.Arm = "35";
            
            vm.GenerateReportCommand.Execute(null);
            
            var report = vm.ReportText;
            Assert.Contains("您的BMI指数为: 31.25", report);
            Assert.Contains("评价: 您的体重状况属于【肥胖】范围。", report);
            Assert.Contains("建议：建议咨询专业人士，制定科学的减重计划", report);

            Assert.Contains("评价: 您的体脂率属于【过高】范围。", report);
            
            Assert.Contains("您的腰臀比为: 0.90", report);
            Assert.Contains("评价: 您患心血管疾病的风险为【高风险】。", report);
        }
        
        [Fact]
        public void GenerateReport_WithBoundaryBmiValues_GeneratesCorrectStatus()
        {
            var vm1 = CreateViewModel();
            vm1.Height = "175";
            vm1.Weight = "56.4"; 
            vm1.BodyFatPercentage = "15";
            vm1.Gender = "男";
            vm1.Chest = "90";
            vm1.Waist = "75";
            vm1.Hips = "90";
            vm1.Arm = "28";
            vm1.GenerateReportCommand.Execute(null);
            Assert.Contains("【偏瘦】", vm1.ReportText);
            
            var vm2 = CreateViewModel();
            vm2.Height = "175";
            vm2.Weight = "56.7"; 
            vm2.BodyFatPercentage = "15";
            vm2.Gender = "男";
            vm2.Chest = "90";
            vm2.Waist = "75";
            vm2.Hips = "90";
            vm2.Arm = "28";
            vm2.GenerateReportCommand.Execute(null);
            Assert.Contains("【正常】", vm2.ReportText);
            
            var vm3 = CreateViewModel();
            vm3.Height = "175";
            vm3.Weight = "73.1"; 
            vm3.BodyFatPercentage = "20";
            vm3.Gender = "男";
            vm3.Chest = "95";
            vm3.Waist = "85";
            vm3.Hips = "95";
            vm3.Arm = "30";
            vm3.GenerateReportCommand.Execute(null);
            Assert.Contains("【正常】", vm3.ReportText);
            
            var vm4 = CreateViewModel();
            vm4.Height = "175";
            vm4.Weight = "73.5"; 
            vm4.BodyFatPercentage = "22";
            vm4.Gender = "男";
            vm4.Chest = "98";
            vm4.Waist = "88";
            vm4.Hips = "98";
            vm4.Arm = "31";
            vm4.GenerateReportCommand.Execute(null);
            Assert.Contains("【超重】", vm4.ReportText);
        }
    }