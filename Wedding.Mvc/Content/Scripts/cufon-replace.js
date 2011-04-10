function cufonize() {
    Cufon.replace('.menu li a, h2, .title, .title1, .title3', { fontFamily: 'Swiss 921', hover: true });
    Cufon.replace('h1 span, h2 span', { fontFamily: 'Swiss 921' });
    Cufon.replace('h3, .link', { fontFamily: 'Swis721 BT', textShadow: '#fff 1px 1px' });
    Cufon.replace('.footer, .welcome', { fontFamily: 'Swiss 921', hover: true });
    Cufon.replace('.login-label, .user-label', { fontFamily: 'Swiss 921', textShadow: '#fff 1px 1px' });
}

cufonize();