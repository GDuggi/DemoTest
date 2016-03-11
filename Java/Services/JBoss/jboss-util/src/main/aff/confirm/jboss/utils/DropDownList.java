 package aff.confirm.jboss.utils;

 import java.io.Serializable;

 public class DropDownList
   implements Cloneable, Serializable
 {
   private String value = "";
   String[] tags;

   public DropDownList()
   {
   }

   public DropDownList(String value, String[] tags)
   {
     this.value = value;
     this.tags = tags;
   }

   public String getValue() {
     return this.value;
   }

   public void setValue(String value) {
     this.value = value;
   }

   public String[] getTags()
   {
     return this.tags;
   }

   public void setTags(String[] tags) {
     this.tags = tags;
   }

   public String getTagsString() {
     String result = "";
     for (int i = 0; i < this.tags.length; i++)
       result = result + "," + this.tags[i];
     if (result.length() > 0)
       result = result.substring(1);
     return result;
   }

   public void setTags(String tags) {
     this.tags = tags.split(",");
   }

   public Object clone() {
     String[] cloneTags = new String[this.tags.length];
     for (int i = 0; i < this.tags.length; i++) {
       cloneTags[i] = this.tags[i];
     }
     DropDownList clone = new DropDownList(this.value, cloneTags);
     return clone;
   }
 }

