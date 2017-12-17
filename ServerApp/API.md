# API Routes

## Account

### Register
`POST /api/Account/Register`

- Email
- Password

Returns: JSON

- ModelState errors:
```
[
    {
        "exception": null,
        "errorMessage": "The Email field is required."
    },
    {
        "exception": null,
        "errorMessage": "The Password field is required."
    }
]
```
- Failed IdentityResult:
```
{
    "succeeded": false,
    "errors": [
        {
            "code": "DuplicateUserName",
            "description": "User name 'test@domain.com' is already taken."
        }
    ]
}
```
- String: JWT Token

### Login
`POST /api/Account/Login`
- Email
- Password

Returns: JSON

- ModelState errors:
```
[
    {
        "exception": null,
        "errorMessage": "The Email field is required."
    },
    {
        "exception": null,
        "errorMessage": "The Password field is required."
    }
]
```
- Failed IdentityResult:
```
{
    "succeeded": false,
    "isLockedOut": false,
    "isNotAllowed": false,
    "requiresTwoFactor": false
}
```
- String: JWT Token

### Logout
`GET /api/Account/Logout`


## Lists

### Owned or subscribed list By ID
`GET /api/Lists/{id}`

- JWT Token of list owner or participant in Headers

Returns: JSON List with all items

### Edit list
`PUT /api/Lists/{id}`

- JWT Token of list owner in Headers
- List model

### Edit list
`PATCH /api/Lists/{id}`

- JWT Token of list owner in Headers
- Partial list model like this:

```
[
	{
		"Op": "replace",
		"Path": "Name",
		"Value": "test"
	}
]
```

### New list
`POST /api/Lists/{id}`

- JWT Token in Headers
- List model

### Delete List
`DELETE /api/Lists/{id}`

- JWT Token of list owner in Headers


## Items

### Toggle checkmark

`PUT /api/Items/{id}`

- JWT Token of list subscriber in Headers

### New item
`POST /api/Items/{id}`

- JWT Token of list participant in Headers
- Item model

### Delete Item
`DELETE /api/Items/{id}`

- JWT Token of item owner in Headers


## Users

### Get Logged in User

`GET /api/Users/`

Returns: JSON User

### Get User

`GET /api/Users/{id}`

Returns: JSON User with populated public OwningLists field

### All owned lists
`GET /api/Users/Lists`

- JWT Token in Headers

Returns: JSON [List] of logged in user

### All subscribed lists
`GET /api/Users/Subscriptions`

- JWT Token in Headers

Returns: JSON [List] of logged in user