using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Forms;

namespace AKSoftware.Localization.MultiLanguages
{
    public static class ValidationLocalization
    {
        public static bool ValidateModel<T>(T model, ValidationMessageStore validationMessageStore, ILanguageContainerService lc) where T : class, new()
        {
            bool valid = true;
            var type = model.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(true);
                valid = ValidateAttributes(model, validationMessageStore, lc, attributes, property, valid);
            }

            return valid;
        }

        private static bool ValidateAttributes<T>(T model, ValidationMessageStore validationMessageStore,
            ILanguageContainerService lc, object[] attributes, PropertyInfo property, bool valid) where T : class, new()
        {
            foreach (var attribute in attributes)
            {
                if (attribute is RequiredAttribute requiredAttribute)
                {
                    if (!ValidateRequired(model, validationMessageStore, lc, property, requiredAttribute))
                        valid = false;
                }
                else if (attribute is MaxLengthAttribute maxLengthAttribute)
                {
                    if (!ValidateMaxLength(model, validationMessageStore, lc, property, maxLengthAttribute))
                        valid = false;
                }
                else if (attribute is StringLengthAttribute stringLengthAttribute)
                {
                    if (!ValidateStringLength(model, validationMessageStore, lc, property, stringLengthAttribute))
                        valid = false;
                }
                else if (attribute is RangeAttribute rangeAttribute)
                {
                    if (!ValidateRange(model, validationMessageStore, lc, property, rangeAttribute))
                        valid = false;
                }
                else if (attribute is RegularExpressionAttribute regularExpressionAttribute)
                {
                    if (!ValidateRegularExpression(model, validationMessageStore, lc, property, regularExpressionAttribute))
                        valid = false;
                }
                else if (attribute is CompareAttribute compareAttribute)
                {
                    if (!ValidateCompare(model, validationMessageStore, lc, property, compareAttribute))
                        valid = false;
                }
                else if (attribute is EmailAddressAttribute emailAddressAttribute)
                {
                    if (!ValidateEmail(model, validationMessageStore, lc, property, emailAddressAttribute))
                        valid = false;
                }
                else if (attribute is PhoneAttribute phoneAttribute)
                {
                    if (!ValidatePhone(model, validationMessageStore, lc, property, phoneAttribute))
                        valid = false;
                }
                else if (attribute is CreditCardAttribute creditCardAttribute)
                {
                    if (!ValidateCreditCard(model, validationMessageStore, lc, property, creditCardAttribute))
                        valid = false;
                }
                else if (attribute is UrlAttribute urlAttribute)
                {
                    if (!ValidateUrl(model, validationMessageStore, lc, property, urlAttribute))
                        valid = false;
                }

                if (!valid)
                    break;
            }

            return valid;
        }

        private static bool ValidateMaxLength<T>(T model, ValidationMessageStore validationMessageStore, ILanguageContainerService lc, PropertyInfo property, MaxLengthAttribute maxLengthAttribute) where T : class, new()
        {
            if (property.PropertyType != typeof(string))
                return true;

            if (property.GetValue(model) != null && property.GetValue(model).ToString().Length > maxLengthAttribute.Length)
            {
                string languageKey = $"{DataAttributeParsing.MaxLengthPrefix}{property.Name}{maxLengthAttribute.Length}";
                string translation = lc.Keys[languageKey];

                if (!string.IsNullOrEmpty(translation) && translation != languageKey)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), lc.Keys[languageKey]);
                }
                else if (maxLengthAttribute.ErrorMessage != null)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), maxLengthAttribute.ErrorMessage);
                }
                else
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), $"{property.Name} should not exceed {maxLengthAttribute.Length} characters");
                }

                return false;
            }

            return true;
        }

        private static bool ValidateStringLength<T>(T model, ValidationMessageStore validationMessageStore, ILanguageContainerService lc, PropertyInfo property, StringLengthAttribute stringLengthAttribute) where T : class, new()
        {
            if (property.PropertyType != typeof(string))
                return true;

            var value = property.GetValue(model)?.ToString();
            if (value != null && (value.Length < stringLengthAttribute.MinimumLength || value.Length > stringLengthAttribute.MaximumLength))
            {
                string languageKey = $"{DataAttributeParsing.StringLengthPrefix}{property.Name}{stringLengthAttribute.MinimumLength}{stringLengthAttribute.MaximumLength}";
                string translation = lc.Keys[languageKey];

                if (!string.IsNullOrEmpty(translation) && translation != languageKey)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), lc.Keys[languageKey]);
                }
                else if (stringLengthAttribute.ErrorMessage != null)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), stringLengthAttribute.ErrorMessage);
                }
                else
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), $"{property.Name} should be between {stringLengthAttribute.MinimumLength} and {stringLengthAttribute.MaximumLength} characters");
                }

                return false;
            }

            return true;
        }

        private static bool ValidateRange<T>(T model, ValidationMessageStore validationMessageStore, ILanguageContainerService lc, PropertyInfo property, RangeAttribute rangeAttribute) where T : class, new()
        {
            var value = property.GetValue(model);
            if (value == null)
                return true;

            double doubleValue;
            try
            {
                doubleValue = Convert.ToDouble(value);
            }
            catch (Exception)
            {
                return true;
            }

            if (doubleValue < Convert.ToDouble(rangeAttribute.Minimum) || doubleValue > Convert.ToDouble(rangeAttribute.Maximum))
            {
                string languageKey = $"{DataAttributeParsing.RangePrefix}{property.Name}{rangeAttribute.Minimum}{rangeAttribute.Maximum}";
                string translation = lc.Keys[languageKey];

                if (!string.IsNullOrEmpty(translation) && translation != languageKey)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), lc.Keys[languageKey]);
                }
                else if (rangeAttribute.ErrorMessage != null)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), rangeAttribute.ErrorMessage);
                }
                else
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), $"{property.Name} should be between {rangeAttribute.Minimum} and {rangeAttribute.Maximum}");
                }

                return false;
            }

            return true;
        }

        private static bool ValidateRegularExpression<T>(T model, ValidationMessageStore validationMessageStore, ILanguageContainerService lc, PropertyInfo property, RegularExpressionAttribute regularExpressionAttribute) where T : class, new()
        {
            if (property.PropertyType != typeof(string))
                return true;

            var value = property.GetValue(model)?.ToString();
            if (value != null && !Regex.IsMatch(value, regularExpressionAttribute.Pattern))
            {
                string languageKey = $"{DataAttributeParsing.RegularExpressionPrefix}{property.Name}{regularExpressionAttribute.Pattern}";
                string translation = lc.Keys[languageKey];

                if (!string.IsNullOrEmpty(translation) && translation != languageKey)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), lc.Keys[languageKey]);
                }
                else if (regularExpressionAttribute.ErrorMessage != null)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), regularExpressionAttribute.ErrorMessage);
                }
                else
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), $"{property.Name} is not in the correct format");
                }

                return false;
            }

            return true;
        }

        private static bool ValidateCompare<T>(T model, ValidationMessageStore validationMessageStore, ILanguageContainerService lc, PropertyInfo property, CompareAttribute compareAttribute) where T : class, new()
        {
            var currentValue = property.GetValue(model)?.ToString();
            var compareProperty = model.GetType().GetProperty(compareAttribute.OtherProperty);
            var compareValue = compareProperty?.GetValue(model)?.ToString();

            if (currentValue != compareValue)
            {
                string languageKey = $"{DataAttributeParsing.ComparePrefix}{property.Name}{compareAttribute.OtherProperty}";
                string translation = lc.Keys[languageKey];

                if (!string.IsNullOrEmpty(translation) && translation != languageKey)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), lc.Keys[languageKey]);
                }
                else if (compareAttribute.ErrorMessage != null)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), compareAttribute.ErrorMessage);
                }
                else
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), $"{property.Name} must be equal to {compareAttribute.OtherProperty}");
                }

                return false;
            }

            return true;
        }

        private static bool ValidateEmail<T>(T model, ValidationMessageStore validationMessageStore, ILanguageContainerService lc, PropertyInfo property, EmailAddressAttribute emailAddressAttribute) where T : class, new()
        {
            if (property.PropertyType != typeof(string))
                return true;

            var value = property.GetValue(model)?.ToString();
            if (value != null && !new EmailAddressAttribute().IsValid(value))
            {
                string languageKey = $"{DataAttributeParsing.EmailAddressPrefix}{property.Name}";
                string translation = lc.Keys[languageKey];

                if (!string.IsNullOrEmpty(translation) && translation != languageKey)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), lc.Keys[languageKey]);
                }
                else if (emailAddressAttribute.ErrorMessage != null)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), emailAddressAttribute.ErrorMessage);
                }
                else
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), $"{property.Name} is not a valid email address");
                }

                return false;
            }

            return true;
        }

        private static bool ValidatePhone<T>(T model, ValidationMessageStore validationMessageStore, ILanguageContainerService lc, PropertyInfo property, PhoneAttribute phoneAttribute) where T : class, new()
        {
            if (property.PropertyType != typeof(string))
                return true;

            var value = property.GetValue(model)?.ToString();
            if (value != null && !new PhoneAttribute().IsValid(value))
            {
                string languageKey = $"{DataAttributeParsing.PhonePrefix}{property.Name}";
                string translation = lc.Keys[languageKey];

                if (!string.IsNullOrEmpty(translation) && translation != languageKey)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), lc.Keys[languageKey]);
                }
                else if (phoneAttribute.ErrorMessage != null)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), phoneAttribute.ErrorMessage);
                }
                else
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), $"{property.Name} is not a valid phone number");
                }

                return false;
            }

            return true;
        }


        private static bool ValidateCreditCard<T>(T model, ValidationMessageStore validationMessageStore, ILanguageContainerService lc, PropertyInfo property, CreditCardAttribute creditCardAttribute) where T : class, new()
        {
            if (property.PropertyType != typeof(string))
                return true;

            var value = property.GetValue(model)?.ToString();
            if (value != null && !new CreditCardAttribute().IsValid(value))
            {
                string languageKey = $"{DataAttributeParsing.CreditCardPrefix}{property.Name}";
                string translation = lc.Keys[languageKey];

                if (!string.IsNullOrEmpty(translation) && translation != languageKey)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), lc.Keys[languageKey]);
                }
                else if (creditCardAttribute.ErrorMessage != null)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), creditCardAttribute.ErrorMessage);
                }
                else
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), $"{property.Name} is not a valid credit card number");
                }

                return false;
            }

            return true;
        }

        private static bool ValidateUrl<T>(T model, ValidationMessageStore validationMessageStore, ILanguageContainerService lc, PropertyInfo property, UrlAttribute urlAttribute) where T : class, new()
        {
            if (property.PropertyType != typeof(string))
                return true;

            var value = property.GetValue(model)?.ToString();
            if (value != null && !new UrlAttribute().IsValid(value))
            {
                string languageKey = $"{DataAttributeParsing.UrlPrefix}{property.Name}";
                string translation = lc.Keys[languageKey];

                if (!string.IsNullOrEmpty(translation) && translation != languageKey)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), lc.Keys[languageKey]);
                }
                else if (urlAttribute.ErrorMessage != null)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), urlAttribute.ErrorMessage);
                }
                else
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), $"{property.Name} is not a valid URL");
                }

                return false;
            }

            return true;
        }
        private static bool ValidateRequired<T>(T model, ValidationMessageStore validationMessageStore, ILanguageContainerService lc, PropertyInfo property, RequiredAttribute requiredAttribute) where T : class, new()
        {
            if (property.PropertyType != typeof(string))
                return true;

            if (property.GetValue(model) == null || string.IsNullOrWhiteSpace(property.GetValue(model).ToString()))
            {
                string languageKey = $"{DataAttributeParsing.RequiredPrefix}{property.Name}";
                string translation = lc.Keys[languageKey];

                if (!string.IsNullOrEmpty(translation) && translation != languageKey)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), lc.Keys[languageKey]);
                }
                else if (requiredAttribute.ErrorMessage != null)
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), requiredAttribute.ErrorMessage);
                }
                else
                {
                    validationMessageStore.Add(new FieldIdentifier(model, property.Name), $"{property.Name} is required");
                }

                return false;
            }

            return true;
        }
    }
}
