# aws_cognito_aspnetcore_client

<h3>Integrating ASP .NET 8 MVC with AWS Cognito</h3>

<hr>

To get started with Authenticating a ASP .NET 8 MVC application with AWS Cognito. We need to create an user pool first.

A user pool is a collection of users, groups and other settings.

To create a user pool head over to the AWS console and type in the pool name like in the below screenshot.

[Create user pool in AWS Console 1](/assets/create_user_pool_1.png)

Add in optional attributes for signup like `email`, `family_name`, `given_name` and `phone_number`

[Attributes for signup](/assets/create_user_pool_2.png)

Configure the login and logout URL(s).

Login URL - `http://localhost:5255/signin-oidc` and Logout URL - `http://localhost:5255/`

[Configure login and logout URL(s)](/assets/create_user_pool_3.png)

<hr>

To login to a protected URL, we will be redirected first to the AWS Cognito Page.

[Accessing protected URL](/assets/home_page_sign_in_flow_1.png)

[Redirection to Sign in page and email prompt](/assets/home_page_sign_in_flow_2.png)

[Redirection to Sign in page and password prompt](/assets/home_page_sign_in_flow_2.png)

Once we have successfully logged in, the claims will be visible along with the `id_token` and `access_token`.

<hr>

Since the self-registration functionality is enabled by default in the AWS Cognito, users will be able to sign up by default.

For signing up the user will be prompted with `email` and `password`. Passwords have certain password policies, which we can edit in the AWS Cognito User Pool settings to our requirement.

[Signup flow email and password prompt](/assets/signup_flow_1.png)

Once we enter the `email` and `password` Cognito will send a confirmation code to our email to confirm our email address.

[Email confirmation prompt](/assets/signup_flow_2.png)

Once we enter the confirmation code to our email to confirm our email address Cognito will verify our user.

[Email confirmation prompt code](/assets/signup_flow_3.png)

Once we successfully entered the confirmation code we will be redirected to the home page or redirect URL.

[Post signup flow](/assets/signup_flow_4.png)