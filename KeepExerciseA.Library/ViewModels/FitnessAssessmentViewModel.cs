using System;
using System.Globalization;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthAssessment.Models;
using KeepExerciseA.Library.Services;
using KeepExerciseA.Library.ViewModels;

namespace KeepExerciseA.Library.ViewModels;

public partial class FitnessAssessmentViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _height = string.Empty;

    [ObservableProperty]
    private string _weight = string.Empty;

    [ObservableProperty]
    private string _bodyFatPercentage = string.Empty;

    [ObservableProperty]
    private string _gender = "男";

    [ObservableProperty]
    private string _chest = string.Empty;

    [ObservableProperty]
    private string _waist = string.Empty;

    [ObservableProperty]
    private string _hips = string.Empty;

    [ObservableProperty]
    private string _arm = string.Empty;

    [ObservableProperty]
    private string _reportText = "请填写您的身体数据，然后点击“生成报告”按钮。";

    [RelayCommand]
    private void GenerateReport()
    {
        // 1. 数据验证和解析
        if (!TryParseInputs(out HealthData? data))
        {
            ReportText = "输入错误：请确保所有字段都填写了有效的数字。";
            return;
        }

        // 2. 调用AI分析（模拟）
        ReportText = SimulateAiAnalysis(data);
    }

    private bool TryParseInputs(out HealthData? data)
    {
        data = null;
        var culture = CultureInfo.InvariantCulture;

        if (!double.TryParse(Height, culture, out var h) || h <= 0) return false;
        if (!double.TryParse(Weight, culture, out var w) || w <= 0) return false;
        if (!double.TryParse(BodyFatPercentage, culture, out var bf) || bf < 0 || bf > 100) return false;
        if (!double.TryParse(Chest, culture, out var c) || c <= 0) return false;
        if (!double.TryParse(Waist, culture, out var wa) || wa <= 0) return false;
        if (!double.TryParse(Hips, culture, out var hi) || hi <= 0) return false;
        if (!double.TryParse(Arm, culture, out var a) || a <= 0) return false;

        data = new HealthData
        {
            Height = h,
            Weight = w,
            BodyFatPercentage = bf,
            Gender = Gender,
            Chest = c,
            Waist = wa,
            Hips = hi,
            Arm = a
        };

        return true;
    }

    private string SimulateAiAnalysis(HealthData data)
    {
        var sb = new StringBuilder();
        sb.AppendLine("==============================");
        sb.AppendLine("       您的健康评估报告");
        sb.AppendLine("==============================");
        sb.AppendLine($"报告生成时间: {DateTime.Now:yyyy年MM月dd日 HH:mm}");
        sb.AppendLine();

        // --- 数据摘要 ---
        sb.AppendLine("【一、数据摘要】");
        sb.AppendLine($"性别: {data.Gender}");
        sb.AppendLine($"身高: {data.Height:F1} cm");
        sb.AppendLine($"体重: {data.Weight:F1} kg");
        sb.AppendLine($"体脂率: {data.BodyFatPercentage:F1}%");
        sb.AppendLine($"胸围: {data.Chest:F1} cm");
        sb.AppendLine($"腰围: {data.Waist:F1} cm");
        sb.AppendLine($"臀围: {data.Hips:F1} cm");
        sb.AppendLine($"臂围: {data.Arm:F1} cm");
        sb.AppendLine();

        // --- BMI 分析 ---
        sb.AppendLine("【二、BMI（身体质量指数）分析】");
        double heightInMeters = data.Height / 100.0;
        double bmi = data.Weight / (heightInMeters * heightInMeters);
        sb.AppendLine($"您的BMI指数为: {bmi:F2}");

        string bmiStatus = bmi switch
        {
            < 18.5 => "偏瘦",
            < 24 => "正常",
            < 28 => "超重",
            _ => "肥胖"
        };
        sb.AppendLine($"评价: 您的体重状况属于【{bmiStatus}】范围。");
        sb.AppendLine(GetBmiAdvice(bmi));
        sb.AppendLine();

        // --- 体脂率分析 ---
        sb.AppendLine("【三、体脂率分析】");
        sb.AppendLine($"您的体脂率为: {data.BodyFatPercentage:F1}%");
        string bodyFatStatus = (data.Gender == "男") switch
        {
            true => data.BodyFatPercentage switch
            {
                < 10 => "过低",
                < 20 => "标准",
                < 25 => "偏高",
                _ => "过高"
            },
            false => data.BodyFatPercentage switch
            {
                < 15 => "过低",
                < 25 => "标准",
                < 30 => "偏高",
                _ => "过高"
            }
        };
        sb.AppendLine($"评价: 您的体脂率属于【{bodyFatStatus}】范围。");
        sb.AppendLine(GetBodyFatAdvice(data.BodyFatPercentage, data.Gender));
        sb.AppendLine();

        // --- 腰臀比分析 ---
        sb.AppendLine("【四、腰臀比（WHR）分析】");
        double whr = data.Waist / data.Hips;
        sb.AppendLine($"您的腰臀比为: {whr:F2}");
        string whrRisk = (data.Gender == "男") switch
        {
            true => whr switch { < 0.9 => "低风险", < 1.0 => "中等风险", _ => "高风险" },
            false => whr switch { < 0.8 => "低风险", < 0.85 => "中等风险", _ => "高风险" }
        };
        sb.AppendLine($"评价: 您患心血管疾病的风险为【{whrRisk}】。");
        sb.AppendLine(GetWhrAdvice(whrRisk));
        sb.AppendLine();

        // --- 综合建议 ---
        sb.AppendLine("【五、综合健康建议】");
        sb.AppendLine("基于以上数据，我们为您提供以下个性化建议：");
        sb.AppendLine();
        sb.AppendLine("1. 饮食方面：");
        if (bmi > 24 || data.BodyFatPercentage > 25)
        {
            sb.AppendLine("   - 建议适当控制总热量摄入，增加蔬菜和蛋白质的比例，减少高糖、高油食物。");
        }
        else if (bmi < 18.5)
        {
            sb.AppendLine("   - 建议增加营养摄入，保证充足的优质蛋白质和健康脂肪，可适当增加餐次。");
        }
        else
        {
            sb.AppendLine("   - 您的体重目前很理想，请继续保持均衡饮食，注意食物多样性。");
        }
        sb.AppendLine();
        sb.AppendLine("2. 运动方面：");
        if (data.BodyFatPercentage > 25)
        {
            sb.AppendLine("   - 建议将有氧运动（如慢跑、游泳、骑行）与力量训练相结合，每周至少3-5次，每次30分钟以上。");
        }
        else
        {
            sb.AppendLine("   - 建议保持规律运动，可以尝试增加力量训练来塑造体型和提高基础代谢。");
        }
        sb.AppendLine();
        sb.AppendLine("3. 生活习惯：");
        sb.AppendLine("   - 保证每晚7-8小时的充足睡眠，有助于身体恢复和激素平衡。");
        sb.AppendLine("   - 学会管理压力，长期压力会影响健康，可以尝试冥想或瑜伽。");
        sb.AppendLine();
        return sb.ToString();
    }

    private string GetBmiAdvice(double bmi)
    {
        return bmi switch
        {
            < 18.5 => "建议：适当增加营养摄入，进行适度的力量训练以增加肌肉量。",
            < 24 => "建议：恭喜您！请继续保持健康的生活方式，均衡饮食，规律运动。",
            < 28 => "建议：注意控制饮食，减少高热量食物摄入，并增加有氧运动。",
            _ => "建议：建议咨询专业人士，制定科学的减重计划，重点关注饮食和运动管理。"
        };
    }

    private string GetBodyFatAdvice(double bodyFat, string gender)
    {
        bool isMale = gender == "男";
        bool isHigh = (isMale && bodyFat > 20) || (!isMale && bodyFat > 25);

        if (isHigh)
        {
            return "建议：体脂率偏高，增加力量训练来提升肌肉量是降低体脂的有效途径，同时配合有氧运动。";
        }
        return "建议：您的体脂率在健康范围内，继续保持。";
    }

    private string GetWhrAdvice(string riskLevel)
    {
        return riskLevel switch
        {
            "高风险" => "建议：中心性肥胖（苹果型身材）与健康风险关联较大，请特别关注腰围，通过全身性减脂来改善。",
            "中等风险" => "建议：风险适中，可以通过调整饮食和增加运动来进一步降低风险。",
            _ => "建议：您的脂肪分布较为健康，请继续保持。"
        };
    }
}
