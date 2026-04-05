export type User = {
    id: string;
    userName: string;
    email: string;
    token: string;
    imageUrl?: string;
    roles: string[];
}

export type LoginCreds = {
    email: string;
    password: string;
}

export type RegisterCreds = {
    userName: string;
    email: string;
    password: string;
    gender: string;
    dateOfBirth: string;
    city: string;
    country: string;
}