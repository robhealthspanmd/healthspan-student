using healthspanmd.core.CQRS.Metrics;
using System;
using System.Linq;

namespace healthspanmd.web.Models.Metric
{
    public static class MetricViewModel_Mapping
    {
        public static MetricModel ToMetricModel(this MetricViewModel viewModel)
        {
            var model = new MetricModel
            {
                MetricId = viewModel.MetricId,
                Name = viewModel.Name,
                Description = viewModel.Description,
                DataType = (MetricDataType)viewModel.DataType,
                AllowMultipleValues = viewModel.AllowMultipleValues,
                IsActive = viewModel.IsActive,
                Frequency = (MetricFrequencyType)viewModel.Frequency,
                IsAlphaSelectNumeric = viewModel.IsAlphaSelectNumeric,
                IsPositivePolarity = viewModel.IsPositivePolarity,
                Threshold = string.IsNullOrEmpty(viewModel.Threshold) ? null : Convert.ToDouble(viewModel.Threshold),
                Threshold2 = string.IsNullOrEmpty(viewModel.Threshold2) ? null : Convert.ToDouble(viewModel.Threshold2)
            };

            switch (model.DataType)
            {
                case MetricDataType.Numeric_Decimal:
                case MetricDataType.Numeric_Integer:
                case MetricDataType.Numeric_Integer_Dial:
                case MetricDataType.Numeric_Integer_Slider:
                case MetricDataType.Numeric_Integer_2Values:
                    model.MinValue = string.IsNullOrEmpty(viewModel.MinValue) ? null : Convert.ToDouble(viewModel.MinValue);
                    model.MaxValue = string.IsNullOrEmpty(viewModel.MaxValue) ? null : Convert.ToDouble(viewModel.MaxValue);
                    break;
                   
            }

            if (viewModel.SelectItems != null)
                model.SelectItems = viewModel.SelectItems.Select(i => i.ToMetricSelectItemModel()).ToList();

            return model;
        }

        public static MetricViewModel ToMetricViewModel(this MetricModel model)
        {
            var viewModel = new MetricViewModel
            {
                MetricId = model.MetricId,
                Name = model.Name,
                Description = model.Description,
                DataType = (int)model.DataType,
                AllowMultipleValues = model.AllowMultipleValues,
                IsActive = model.IsActive,
                Frequency = (int)model.Frequency,
                Threshold = model.Threshold.HasValue ? model.Threshold.ToString() : "",
                IsAlphaSelectNumeric = model.IsAlphaSelectNumeric,
                IsPositivePolarity = model.IsPositivePolarity,
                Threshold2 = model.Threshold2.HasValue ? model.Threshold2.ToString() : ""
            };
            if (model.MinValue.HasValue)
                viewModel.MinValue = model.MinValue.Value.ToString();
            if (model.MaxValue.HasValue)
                viewModel.MaxValue = model.MaxValue.Value.ToString();

            if (model.SelectItems != null)
                viewModel.SelectItems = model.SelectItems.Select(i => i.ToMetricSelectItemViewModel()).ToList();
            

            return viewModel;
        }


        public static MetricSelectItemViewModel ToMetricSelectItemViewModel(this MetricSelectItemModel i)
        {
            return new MetricSelectItemViewModel
            {
                ItemDisplayText = i.ItemDisplayText,
                ItemValue = i.ItemValue,
                MetricId = i.MetricId,
                MetricSelectItemId = i.MetricSelectItemId,
                SortOrder = i.SortOrder,
            };
        }

        public static MetricSelectItemModel ToMetricSelectItemModel(this MetricSelectItemViewModel i)
        {
            return new MetricSelectItemModel
            {
                ItemDisplayText = i.ItemDisplayText,
                ItemValue = i.ItemValue,
                MetricId = i.MetricId,
                MetricSelectItemId = i.MetricSelectItemId,
                SortOrder = i.SortOrder,
            };
        }

    }
}
