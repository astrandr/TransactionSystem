The implementation uses two layers: UI and Core.

The UI accepts user input in Program.Main and processes it through ShellProcessor. To exit the application, enter 'quit'.

ShellProcessor iterates through the command interpreters defined in Commands\Interpreters.

Each interpreter attempts to parse the input line into a command and returns an InterpreterResult. If the input does not match the expected parameters, it returns InvalidParameters in the result object.

If a command is recognized and successfully created, its execution method is called with the AccountService instance defined in the Core layer.

AccountService is constructed with an AccountRepository instance.

The AccountService class implements the following operations: CreateAccount, Deposit, GetBalance, Withdraw, and Transfer.

AccountRepository provides in-memory storage for the accounts.

NOTE: The current concurrency handling is based on the assumption that accounts cannot be deleted, as account deletion functionality is not required by the assignment.