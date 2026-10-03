The implementation uses two layers: UI and Core

The UI accepts the user input lines in Program:Main and parses them in ShellProcessor. To exit the application enter 'quit'.

The ShellProcessor iterates command interpreters defined in Commands\Interpreters.

Each interpreter tries to parse the text line into command and returns InterpreterResult. If there is mismatch with expected parameters it returns InvalidParameters in the result object.

If command is recognized and successfuly create it calls its execution method passing AccountService object defined in the Core layer.

AccountService is constructed with AccountRepository object. 

The AccountService class implements the operations: CreateAccount, Deposit, GetBalance, Withdraw and Transfer.

The AccountRepository has in-memory storage for the accounts.

!!! NOTE: Current handling of concurrency is built on the assumption that 'NO ACCOUNTS CAN BE DELETED' as such functionality is not required in the assignment.