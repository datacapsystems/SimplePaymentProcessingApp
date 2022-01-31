# Datacap Systems, Inc. Software Engineer Applicant Assessment

## Introduction
This project, which will be referred to from this point on as the 'Simple Payment Processing App' (or SPPA for short), is intended to assess your ability to add functionality to an existing code base in a way that adheres to its present standard as best as possible. Such a task requires strong code reading and comprehension skills and a considerable amount of tact when modifying and adding code.

In summary, acceptable responses will successfully add the features outlined in this specification. Exceptional responses will add those features in a way that integrates seamlessly and (generally) maintains existing conventions and logic flows, all while displaying superior organization and documentation.

The project given to you has the following properties:
+ Written in C# (Version 10.0)
+ Developed using Visual Studio 2022 (Version 17.0.4, 64-bit)
	+ Created as a C# WPF Application
+ Developed on .NET Core 6.0 LTS
+ Has only one external package installed:
	+ System.Text.Json (by Microsoft)
+ Compiled and run successfully on a Windows 10 machine
+ Compiled and run successfully without any warnings or errors

As given to you, the purpose of the SPPA is to process simple credit transaction requests and calculate a payment processing fee. The SPPA accepts a request and determines whether it should be approved or declined. In the case of an approval, the response indicates the processing fee and that a signature is required. Note that in the case of a declination, the processing fee is always zero and a signature is not required.

The credit request fields and their accompanying rules are as follows:
+ `Amount` - Required field; must be greater than or equal to zero
+ `CardNumber` - Required field; must be exactly 16 characters long
+ `Expiration` - Required field; must be in the format MM/YYYY
+ `CardholderName` - Optional field

After validating the request fields, the SPPA will process the transaction. If it would reach approval, the processing fee is calculated as a percentage of the transaction amount based on the card brand. The card brand is determined by reading the first four digits of the card number.

The relationship between the first four numbers of a card number, a card brand, and the percentage fee are as follows:
+ 1024 | Visa | 4%
+ 2048 | MasterCard | 8%
+ 4096 | Discover | 12%

If the card number does not start with 1024, 2048, or 4096, this indicates that the card brand is unknown, and a fee percentage of 16% should be issued.

When the app is run, a basic UI will appear with an input box, an output box, four checkboxes, and an 'Execute' button. A JSON-formatted transaction request will go in the input box. Any number of the checkboxes may be selected to indicate special processing rules that should be applied. Clicking 'Execute' will process the transaction and print the response to the output box.

A sample credit request and response pairing (with no checkboxes checked) is as follows:

**Request:**
```json
{
  "Amount": 121.98,
  "CardNumber": "2048123412341234",
  "Expiration": "12/2022",
  "CardholderName": "Datacap Systems"
}
```

**Response:**
```json
{
  "CmdStatus": "Approved",
  "TextMessage": "Transaction approved.",
  "ProcessingFee": 9.76,
  "RequiresSignature": true
}
```

The four checkboxes in the UI indicate the following:
- **Check Duplicate** - Requests with the exact same fields as a previous request should be declined.
- **Validate Expiration Date** - Requests that specify an expiration date that has passed will be declined.
- **Require Cardholder Name** - Requests that do not have a valid 'CardholderName' property will be declined.
- **Waive Fee** - The processing fee for successful transactions will be waived.

## Your Task
You will be tasked with adding a few additional features to the SPPA, with some features being more complex than others.

You may modify the source code in whatever way you see fit, so long as it maintains its original functionality in addition to the new features you are adding, and you are abiding by the constraints described in any given subtask.

### Subtask I: Gift Card Transaction Support
You will implement support for gift card transaction processing. Gift card transactions have a slightly different request schema than credit transaction requests.

The gift card request fields and their accompanying rules are as follows:
+ `Amount` - Required field; must be greater than or equal to zero
+ `Account` - Required field; must be exactly 18 characters long
+ `CVV` - Required field; must be exactly 3 characters long
+ `Expiration` - Required field; must be in the format MM/YYYY
+ `CardholderName` - Optional field

Note that a special rule also applies for gift card transactions. The CVV must match the final three characters of the given Account field. If they do not match, the transaction should be declined, and an appropriate message should be given.

Similarly to credit transactions, processing fees for gift card transactions will only given on approvals. Conversely, on declinations, the fee will be zero.

The transaction fees are calculated differently for gift card transactions. The relationship between the first six numbers of an account, a card brand, and the percentage fee are as follows:
+ 001615 | Visa | 5%
+ 061680 | MasterCard | 10%
+ 100101 | Discover | 15%

If the card number does not start with 001615, 061680, or 100101, this indicates that the card brand is unknown, and a fee percentage of 25% should be issued.

A sample gift card request and response pairing (with no checkboxes checked) resulting in an approval is as follows:

**Request:**
```json
{
  "Amount": 103.00,
  "Account": "001615123456123456",
  "CVV": "456",
  "Expiration": "12/2022",
  "CardholderName": "Datacap Systems"
}
```

**Response:**
```json
{
  "CmdStatus": "Approved",
  "TextMessage": "Transaction approved.",
  "ProcessingFee": 5.15,
  "RequiresSignature": true
}
```

A sample gift card request and response pairing (with no checkboxes checked) resulting in an declination due to a mismatched `CVV` is as follows:

**Request:**
```json
{
  "Amount": 103.00,
  "Account": "001615123456123456",
  "CVV": "876",
  "Expiration": "12/2022",
  "CardholderName": "Datacap Systems"
}
```

**Response:**
```json
{
  "CmdStatus": "Declined",
  "TextMessage": "CVV mismatch.",
  "ProcessingFee": 0,
  "RequiresSignature": false
}
```

Note that the responses you provide for the `TextMessage` field are yours to decide. It only matters that they accurately describe the reason for the transaction result.

You are to make use of the existing UI, and not modify it at all for Subtask I (and Subtask II).

Gift card transactions should also remain subject to the aforementioned checkbox modifiers described in the UI. For example, if the 'Waive Fee' box is checked, the fee should be 0 under all circumstances.

### Subtask II: Cardholder Name Validation
Currently, if the 'Require Cardholder Name' checkbox is checked and the `CardholderName` field is provided in the JSON request, the transaction continues processing no matter what is given for `CardholderName`, so long as a value exists.

Instead, you will make it so that the provided value for the cardholder name must be two words.

Examples of acceptable and unacceptable values for `CardholderName` under this new rule are as follows:
+ `"John Doe"` | Acceptable
+ `"D S"` | Acceptable
+ `"Johnny"` | Unacceptable
+ `"My Name Is"` | Unacceptable

This validation functionality should apply to both credit transactions and gift transactions.

This task should also not require any changes to the UI.

### Subtask III: Always Require Signature
Currently, a signature will only be required if a transaction is approved. In this subtask, you will allow the user to indicate whether a signature is always required, or if it's only required on approval.

You will add a checkbox below the existing ones that will allow the user to indicate whether or not a signature is always required. If the checkbox is checked, the response should always have a value of `true` for the `RequiresSignature` field. If the box is unchecked, a signature should only be required if the transaction was approved.

This change should be made for both credit transactions and gift card transactions.

### Extra Credit: Base Processing Fee
If you wish to go above and beyond, you may add a text input below the checkboxes that allows the user to indicate a base processing fee that the calculated fee will be added to. This fee should also be charged on declines.

Note that this base fee should not replace or influence the percentage fee applied to the transaction. The base fee should simply be added to the percentage fee after it is calculated, resulting in the total fee that is returned to the user in the response.

## Deliverables
+ The source code of the modified project
	+ Zipped archive or GitHub/GitLab repository share will suffice
+ If you do not finish within the time limit, please submit what you have anyways

## Tips and Hints

For Subtask I, since the same input box must be used for both credit and gift card transactions, it's important to discern what type of transaction is meant to occur before deserializing the request. You might do this by adding a `TransactionType`field to the request, and checking to see if the request contains the transaction type you're looking for. However, there are many possibilities for acceptable solutions to this problem, and you are encouraged to think about the best course of action.
