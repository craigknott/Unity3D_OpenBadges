var express = require('express');
var router = express.Router();
var nodemailer = require("nodemailer");

/* GET home page. */
router.get('/', function(req, res) {
  res.render('index', { title: 'Express' });
});

/* GET accept badge page */
router.get('/accept', function(req, res) {
  var badgeUrl=req.param("badge");
  res.render("accept", {badge: badgeUrl});
});

/* WebHook POST request handler */
router.post('/hook', function(req, res) {
  var action = req.body.action;
  var info="";
  var emailTo="";
  console.log("Hooked Index");
  var smtpTransport = nodemailer.createTransport("Gmail", {
    auth: {
      user: "user@gmail.com",
      pass: "password"
    }
  });
  switch(action) {
    //review event
    case 'review':
      console.log("Review Case");
      //earner email
      emailTo=req.body.application.learner;
      //build badge name into email
      info+="<p>Your application for the following badge was reviewed:"+ 
        "<strong>"+req.body.application.badge.name+"</strong></p>";

      //respond differently if approved or not
      if(req.body.approved){ 
        info+="<p>Great news - your application was approved!</p>";
        //include link to accept the badge
        // - alter for your url
        info+="<p><a href="+
            "'http://issuersite.com/accept?badge="+
            req.body.application.badge.slug+
            "&earner="+req.body.application.learner+
            "&application="+req.body.application.slug+
            "'>Accept your badge</a></p>";
      }
      else{
        info+="<p>Unfortunately your application was unsuccessful this time. "+
            "You can re-apply for the badge any time though!</p>";
      }
      //review includes multiple feedback and comment items
      info+="<p>The reviewer included feedback for you:</p>";
      info+="<ul>";
      //comment field includes everything in the Finish page in BadgeKit Web app
      info+="<li><em>"+req.body.review.comment+"</em></li>";
      //review items array, one per criteria - build into list
      var reviewItems = req.body.review.reviewItems;
      var r;
      for(r=0; r<reviewItems.length; r++){
        info+="<li><em>"+reviewItems[r].comment+"</em></li>";
        //can also include whether each criteria item was satisfied
      }
      info+="</ul>";
      info+="<p><strong><em>Thanks for applying!</em></strong></p>";
      var mailOptions = {
        from: "CEIT Badge Issuer <user@gmail.com>", 
        to: emailTo,
        subject: "Badge Application Review", 
        generateTextFromHTML: true,
        html: info // html body
      };

      smtpTransport.sendMail(mailOptions, function(err, respo) {
        if (err) {
          console.log(err);
        } else {
          console.log("Sent Message");
        }
      });
      break;
    case 'award':
      console.log("Award Case");
      emailTo=req.body.email;
      info+="<p>You've been awarded this badge:</p>";
      info+="<img src='"+req.body.badge.imageUrl+"' alt='badge'/>";
      info+="<a href='http://winter.ceit.uq.edu.au:3003/accept?badge=http://winter.ceit.uq.edu.au"+req.body.assertionUrl.substr(16)+"'>CLAIM</a>"
      //can offer to push to backpack etc
      var mailOptions = {
        from: "CEIT Badge Issuer <user@gmail.com>", 
        to: emailTo,
        subject: "You have earned a new Badge", 
        generateTextFromHTML: true,
        html: info // html body
      };

      smtpTransport.sendMail(mailOptions, function(err, respo) {
        if (err) {
          console.log(err);
        } else {
          console.log("Sent Message");
        }
      });
      break;
  }
});

module.exports = router;
