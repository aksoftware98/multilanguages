# Validation Localization

Localize DataAnnotation validation messages using `ValidationLocalization`.

## Supported Attributes

`Required`, `MaxLength`, `StringLength`, `Range`, `RegularExpression`, `Compare`, `EmailAddress`, `Phone`, `CreditCard`, `Url`

## How It Works

Each attribute generates a key based on the pattern `{AttributeName}{PropertyName}`. Add these keys to your YAML files with the localized messages.

### Key Naming Patterns

| Attribute | Key Pattern | Example Key |
|-----------|-------------|-------------|
| `[Required]` | `Required{Property}` | `RequiredName` |
| `[MaxLength(255)]` | `MaxLength{Property}{Max}` | `MaxLengthName255` |
| `[StringLength(100, MinimumLength = 5)]` | `StringLength{Property}{Min}{Max}` | `StringLengthName5100` |
| `[Range(1, 100)]` | `Range{Property}{Min}{Max}` | `RangeAge1100` |
| `[Compare("Other")]` | `Compare{Property}{Other}` | `ComparePasswordConfirmPassword` |
| `[EmailAddress]` | `EmailAddress{Property}` | `EmailAddressEmail` |
| `[Phone]` | `Phone{Property}` | `PhonePhoneNumber` |
| `[RegularExpression]` | `RegularExpression{Property}` | `RegularExpressionZipCode` |

## YAML File

```yaml
RequiredName: Name is required
RequiredEmail: Email is required
MaxLengthName255: Name should not exceed 255 characters
EmailAddressEmail: Please enter a valid email address
```

## Blazor Example

```csharp
public partial class ContactUsPage : ComponentBase
{
    [Inject] private ILanguageContainerService Language { get; set; }

    private ContactUs _contactUs;
    private ValidationMessageStore _validationMessageStore;
    private EditContext EC { get; set; }

    protected override void OnInitialized()
    {
        Language.InitLocalizedComponent(this);
        _contactUs = new ContactUs();
        EC = new EditContext(_contactUs);
        _validationMessageStore = new ValidationMessageStore(EC);
    }

    private bool IsValid()
    {
        _validationMessageStore.Clear();
        bool isValid = ValidationLocalization.ValidateModel(
            _contactUs, _validationMessageStore, Language);

        if (!isValid)
        {
            EC.NotifyValidationStateChanged();
            return false;
        }
        return true;
    }

    private void HandleSubmit()
    {
        if (!IsValid())
            return;

        // Handle form submission
    }
}
```

## Auto-Generate Validation Keys

Use [Code Generation](code-generation.md) to automatically scan your models for DataAnnotation attributes and create the corresponding YAML keys.
